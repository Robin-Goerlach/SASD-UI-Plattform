using System.Reflection;
using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.DataDashboardSmokeChecks;

/// <summary>
/// Focused resilience checks for grid-controller behavior that needs a real WinForms
/// synchronization context or deliberate overlapping asynchronous loads.
/// </summary>
internal static class GridControllerResilienceChecks
{
    public static void Run() =>
        RunInStaMessageLoopAsync(async () =>
        {
            await ValidateShrinkingResultRealignmentAsync();
            await ValidateSupersededLoadCancellationAsync();
            await ValidateFailureContractsAsync();
            await ValidateDisposedMutationGuardsAsync();
        }).GetAwaiter().GetResult();

    private static async Task ValidateShrinkingResultRealignmentAsync()
    {
        using var grid = new SasdDataGrid();
        using var pager = new SasdPager();
        var source = new MutablePageSource(
        [
            new ExampleRow(1, "Alpha"),
            new ExampleRow(2, "Beta"),
            new ExampleRow(3, "Gamma"),
            new ExampleRow(4, "Delta"),
            new ExampleRow(5, "Epsilon"),
        ]);
        using var controller = new SasdGridController<ExampleRow>(grid, source, pageSize: 2);
        controller.AttachPager(pager);

        await controller.LoadAsync();
        Button nextButton = GetPrivateField<Button>(pager, "nextButton");
        nextButton.PerformClick();
        nextButton.PerformClick();
        Ensure(controller.PageIndex == 2,
            "Grid-controller setup did not reach the original final page.");

        source.ReplaceRows(
        [
            new ExampleRow(1, "Alpha"),
            new ExampleRow(2, "Beta"),
            new ExampleRow(3, "Gamma"),
        ]);
        source.ClearQueryHistory();

        int loadedEvents = 0;
        controller.PageLoaded += (_, _) => loadedEvents++;
        await controller.LoadAsync();

        Ensure(source.QueryHistory.Count == 2,
            "Shrinking result set did not perform exactly one bounded corrective load.");
        Ensure(source.QueryHistory[0].Offset == 4 && source.QueryHistory[1].Offset == 2,
            "Corrective load did not move from the stale page to the new final page.");
        Ensure(controller.PageIndex == 1 && pager.PageIndex == 1 && pager.TotalCount == 3,
            "Controller and pager did not converge on the new final page after rows were removed.");
        Ensure(grid.DataSource is BindingSource binding &&
               binding.Count == 1 &&
               binding[0] is ExampleRow { Id: 3 },
            "Corrective load did not bind the remaining row from the new final page.");
        Ensure(loadedEvents == 1,
            "A single logical refresh published more than one PageLoaded event during page realignment.");
    }

    private static async Task ValidateSupersededLoadCancellationAsync()
    {
        using var grid = new SasdDataGrid();
        var source = new SupersedingPageSource();
        using var controller = new SasdGridController<ExampleRow>(grid, source, pageSize: 2);

        Task firstLoad = controller.LoadAsync();
        await source.FirstLoadStarted;
        Ensure(controller.IsLoading, "Controller did not report the first pending load.");

        // Starting the second request must cancel, but must not prematurely dispose, the
        // first request's linked CancellationTokenSource. The application-owned source is
        // still unwinding and may legitimately use its token until its Task completes.
        await controller.SearchAsync("newer");
        Ensure(source.FirstCancellationObserved,
            "A newer load did not request cancellation of the superseded data-source call.");
        Ensure(controller.SearchText == "newer",
            "Newer search state was not retained after superseding the pending load.");

        source.ReleaseFirstLoad();
        await EnsureThrowsAsync<OperationCanceledException>(
            () => firstLoad,
            "Superseded load did not complete as cancellation.");
        Ensure(source.LateTokenRegistrationSucceeded,
            "Superseded data source could not finish token-registration cleanup before CTS disposal.");
        Ensure(!controller.IsLoading,
            "Controller remained in loading state after current and superseded loads completed.");

        Ensure(grid.DataSource is BindingSource binding &&
               binding.Count == 1 &&
               binding[0] is ExampleRow { Name: "Current" },
            "Superseded load overwrote the newer page result.");
    }

    private static async Task ValidateFailureContractsAsync()
    {
        using var grid = new SasdDataGrid();
        using var pager = new SasdPager();
        var source = new FailingPageSource();
        using var controller = new SasdGridController<ExampleRow>(grid, source, pageSize: 2);
        controller.AttachPager(pager);

        SasdGridLoadFailedEventArgs? failure = null;
        int failures = 0;
        controller.LoadFailed += (_, args) =>
        {
            failures++;
            failure = args;
        };

        // Pager interaction is an async-void WinForms event path. There is no Task for an
        // application caller to await, so a data-source exception must be converted into the
        // explicit LoadFailed notification rather than escaping the event handler.
        pager.SetState(currentPageIndex: 0, currentPageSize: 2, knownTotalCount: 4);
        Button nextButton = GetPrivateField<Button>(pager, "nextButton");
        nextButton.PerformClick();

        Ensure(failures == 1,
            "Failed pager-driven load did not publish exactly one LoadFailed event.");
        SasdGridLoadFailedEventArgs observedFailure = failure
            ?? throw new InvalidOperationException("Failed pager-driven load did not provide failure event data.");
        Ensure(observedFailure.Exception is InvalidOperationException &&
               observedFailure.Exception.Message == FailingPageSource.ErrorMessage,
            "LoadFailed did not preserve the application data-source exception for technical handling.");
        Ensure(observedFailure.PageIndex == 1 && observedFailure.PageSize == 2,
            "LoadFailed did not describe the page request that failed.");
        Ensure(!controller.IsLoading,
            "Controller remained in loading state after an event-driven failure.");

        using var directGrid = new SasdDataGrid();
        var directSource = new FailingPageSource();
        using var directController = new SasdGridController<ExampleRow>(directGrid, directSource, pageSize: 2);
        int directFailureEvents = 0;
        directController.LoadFailed += (_, _) => directFailureEvents++;

        await EnsureThrowsAsync<InvalidOperationException>(
            () => directController.LoadAsync(),
            "Explicit LoadAsync did not propagate the application data-source failure to its caller.");
        Ensure(directFailureEvents == 0,
            "Explicit LoadAsync incorrectly converted a caller-observable failure into an event notification.");
        Ensure(!directController.IsLoading,
            "Controller remained in loading state after a direct load failure.");
    }

    private static async Task ValidateDisposedMutationGuardsAsync()
    {
        using var grid = new SasdDataGrid();
        var source = new MutablePageSource([new ExampleRow(1, "Alpha")]);
        var controller = new SasdGridController<ExampleRow>(grid, source, pageSize: 10);
        await controller.LoadAsync();

        string? searchBeforeDispose = controller.SearchText;
        IReadOnlyList<SasdSortDescriptor> sortBeforeDispose = controller.Sort;
        int pageBeforeDispose = controller.PageIndex;
        controller.Dispose();

        await EnsureThrowsAsync<ObjectDisposedException>(
            () => controller.SearchAsync("must-not-stick"),
            "Disposed grid controller accepted a search mutation.");
        Ensure(controller.SearchText == searchBeforeDispose && controller.PageIndex == pageBeforeDispose,
            "Rejected search changed controller state after disposal.");

        await EnsureThrowsAsync<ObjectDisposedException>(
            () => controller.SortAsync("Name", SasdSortDirection.Descending),
            "Disposed grid controller accepted a sort mutation.");
        Ensure(ReferenceEquals(controller.Sort, sortBeforeDispose) && controller.PageIndex == pageBeforeDispose,
            "Rejected sort changed controller state after disposal.");
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
                    // Application.Run has installed the real WinForms synchronization context.
                    // These controller checks intentionally contain incomplete awaits, so using
                    // that production-like context avoids the artificial console-test deadlock
                    // documented in tests/AGENTS.md.
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
            Name = "SASD grid controller smoke STA",
        };

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return completion.Task;
    }

    private static T GetPrivateField<T>(object instance, string fieldName)
        where T : class
    {
        FieldInfo? field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (field?.GetValue(instance) is not T value)
        {
            throw new InvalidOperationException($"Expected private field '{fieldName}' was not available for smoke inspection.");
        }

        return value;
    }

    private static async Task EnsureThrowsAsync<TException>(Func<Task> action, string message)
        where TException : Exception
    {
        try
        {
            await action();
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

    private sealed record ExampleRow(int Id, string Name);

    private sealed class MutablePageSource(IEnumerable<ExampleRow> rows) : ISasdDataPageSource<ExampleRow>
    {
        private List<ExampleRow> rows = rows.ToList();

        public List<SasdDataQuery> QueryHistory { get; } = [];

        public Task<SasdDataPage<ExampleRow>> LoadAsync(
            SasdDataQuery query,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            QueryHistory.Add(query);
            IReadOnlyList<ExampleRow> page = rows.Skip(query.Offset).Take(query.Limit).ToArray();
            return Task.FromResult(SasdDataPage<ExampleRow>.Create(page, rows.Count));
        }

        public void ReplaceRows(IEnumerable<ExampleRow> replacement) => rows = replacement.ToList();

        public void ClearQueryHistory() => QueryHistory.Clear();
    }

    private sealed class SupersedingPageSource : ISasdDataPageSource<ExampleRow>
    {
        private readonly TaskCompletionSource firstLoadStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource releaseFirstLoad =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int calls;

        public Task FirstLoadStarted => firstLoadStarted.Task;

        public bool FirstCancellationObserved { get; private set; }

        public bool LateTokenRegistrationSucceeded { get; private set; }

        public async Task<SasdDataPage<ExampleRow>> LoadAsync(
            SasdDataQuery query,
            CancellationToken cancellationToken = default)
        {
            int call = Interlocked.Increment(ref calls);
            if (call > 1)
            {
                return SasdDataPage<ExampleRow>.Create([new ExampleRow(2, "Current")], 1);
            }

            firstLoadStarted.TrySetResult();
            await releaseFirstLoad.Task.ConfigureAwait(false);
            FirstCancellationObserved = cancellationToken.IsCancellationRequested;

            // Register only after the newer request has already cancelled the token. This
            // models application cleanup that still legitimately touches its token before the
            // first source Task completes. Premature CTS disposal used to make this lifecycle
            // ambiguous and could turn expected cancellation into ObjectDisposedException.
            using CancellationTokenRegistration registration = cancellationToken.Register(static () => { });
            LateTokenRegistrationSucceeded = true;
            cancellationToken.ThrowIfCancellationRequested();
            return SasdDataPage<ExampleRow>.Create([new ExampleRow(1, "Stale")], 1);
        }

        public void ReleaseFirstLoad() => releaseFirstLoad.TrySetResult();
    }

    private sealed class FailingPageSource : ISasdDataPageSource<ExampleRow>
    {
        public const string ErrorMessage = "Synthetic page-source failure.";

        public Task<SasdDataPage<ExampleRow>> LoadAsync(
            SasdDataQuery query,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromException<SasdDataPage<ExampleRow>>(new InvalidOperationException(ErrorMessage));
        }
    }
}
