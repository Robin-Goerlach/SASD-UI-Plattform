using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.KeyboardSmokeChecks;

/// <summary>
/// Exercises the search-request debounce on a real WinForms message loop. Timing is used only
/// as a generous deadlock/failure timeout; the assertions do not depend on a performance
/// threshold or on sleeping for an assumed timer duration.
/// </summary>
internal static class SearchBoxRequestChecks
{
    public static void Run() =>
        RunInStaMessageLoopAsync(ValidateRequestContractAsync).GetAwaiter().GetResult();

    private static async Task ValidateRequestContractAsync()
    {
        using var form = new Form
        {
            ClientSize = new Size(480, 120),
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD search debounce smoke",
        };
        using var searchBox = new SasdSearchBox
        {
            DebounceMilliseconds = 25,
            Dock = DockStyle.Top,
        };
        form.Controls.Add(searchBox);
        form.Show();

        int requests = 0;
        string? lastRequestedText = null;
        var firstDebouncedRequest = new TaskCompletionSource<SasdSearchRequestedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        searchBox.SearchRequested += (_, args) =>
        {
            requests++;
            lastRequestedText = args.SearchText;
            if (requests == 1)
            {
                firstDebouncedRequest.TrySetResult(args);
            }
        };

        // All edits happen in one UI-thread turn, so the message-loop timer cannot fire between
        // assignments. When control returns to the message loop only the final quiet value may
        // produce a request.
        searchBox.SearchText = "r";
        searchBox.SearchText = "re";
        searchBox.SearchText = "release";
        Ensure(requests == 0,
            "Debounced search request fired synchronously while text was still being edited.");

        SasdSearchRequestedEventArgs debounced = await firstDebouncedRequest.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Ensure(requests == 1 && debounced.SearchText == "release" && lastRequestedText == "release",
            "Debounce did not collapse rapid edits into one request for the latest search text.");

        searchBox.SearchText = "manual";
        Ensure(requests == 1,
            "Positive debounce unexpectedly raised a request before the UI message loop resumed.");
        searchBox.RequestSearch();
        Ensure(requests == 2 && lastRequestedText == "manual",
            "Explicit RequestSearch did not flush the pending search exactly once.");

        // Changing an active debounce to zero must not silently drop a pending logical search.
        searchBox.SearchText = "policy-change";
        searchBox.DebounceMilliseconds = 0;
        Ensure(requests == 3 && lastRequestedText == "policy-change",
            "Changing a pending debounce to zero did not commit the current search.");

        searchBox.SearchText = "immediate";
        Ensure(requests == 4 && lastRequestedText == "immediate",
            "Zero debounce did not request the edited search synchronously.");

        searchBox.ClearSearch();
        Ensure(requests == 5 && lastRequestedText == string.Empty && searchBox.SearchText.Length == 0,
            "ClearSearch with zero debounce produced a duplicate/missing empty-state request.");

        searchBox.DebounceMilliseconds = 25;
        searchBox.SearchText = "clear-before-timer";
        Ensure(requests == 5,
            "Positive debounce raised a request before explicit clearing could cancel it.");
        searchBox.ClearSearch();
        Ensure(requests == 6 && lastRequestedText == string.Empty,
            "ClearSearch did not replace a pending debounce with one immediate empty-state request.");

        EnsureThrows<ArgumentOutOfRangeException>(
            () => searchBox.DebounceMilliseconds = -1,
            "Search box accepted a negative debounce interval.");

        form.Hide();
    }

    private static Task RunInStaMessageLoopAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            using var context = new ApplicationContext();
            bool started = false;
            EventHandler? idleHandler = null;

            idleHandler = async (_, _) =>
            {
                if (started)
                {
                    return;
                }

                started = true;
                Application.Idle -= idleHandler;
                try
                {
                    // Application.Run installs the normal WinForms synchronization context.
                    // Awaiting SearchRequested therefore yields to the same UI message loop that
                    // owns the component timer instead of simulating timer behavior in test code.
                    await action();
                    completion.TrySetResult();
                }
                catch (Exception exception)
                {
                    completion.TrySetException(exception);
                }
                finally
                {
                    context.ExitThread();
                }
            };

            Application.Idle += idleHandler;
            try
            {
                Application.Run(context);
            }
            catch (Exception exception)
            {
                completion.TrySetException(exception);
            }
            finally
            {
                Application.Idle -= idleHandler;
            }
        })
        {
            IsBackground = true,
            Name = "SASD search debounce smoke STA",
        };

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return completion.Task;
    }

    private static void EnsureThrows<TException>(Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
