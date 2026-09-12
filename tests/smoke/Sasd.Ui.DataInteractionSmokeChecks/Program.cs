using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.DataInteractionSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        Exception? failure = null;

        using var form = new Form
        {
            ClientSize = new Size(640, 420),
            FormBorderStyle = FormBorderStyle.FixedToolWindow,
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD data interaction smoke host",
        };

        using var tree = new TestTreeView
        {
            Dock = DockStyle.Fill,
        };
        form.Controls.Add(tree);

        using var context = new ApplicationContext(form);
        form.Shown += async (_, _) =>
        {
            try
            {
                await ValidateSuccessfulLazyLoadAsync(tree).ConfigureAwait(true);
                await ValidateFailureAndKeyboardRetryAsync(tree).ConfigureAwait(true);
                ValidateRegistrationGuards(tree);
                await ValidateCancellationOnDisposeAsync(form).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                failure = exception;
            }
            finally
            {
                context.ExitThread();
            }
        };

        // The production component intentionally resumes lazy-load continuations on the
        // WinForms UI context. Running a real, invisible ApplicationContext is therefore
        // part of the test contract rather than an implementation convenience.
        Application.Run(context);

        if (failure is not null)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }

        Console.WriteLine("SASD data interaction smoke checks passed.");
        return 0;
    }

    private static async Task ValidateSuccessfulLazyLoadAsync(TestTreeView tree)
    {
        Ensure(tree.AccessibleRole == AccessibleRole.Outline && tree.AccessibleName == "Tree",
            "TreeView does not expose stable outline accessibility semantics.");

        var root = new TreeNode("Customers");
        tree.Nodes.Add(root);

        int loaderCalls = 0;
        var releaseLoader = new TaskCompletionSource<IReadOnlyList<TreeNode>>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var loadedEvent = new TaskCompletionSource<SasdTreeNodeChildrenLoadedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        tree.NodeChildrenLoaded += OnLoaded;
        try
        {
            tree.RegisterLazyNode(root, _ =>
            {
                loaderCalls++;
                return releaseLoader.Task;
            });

            Ensure(root.Nodes.Count == 1,
                "Registering a lazy node did not provide the native expansion placeholder.");

            root.Expand();
            Ensure(loaderCalls == 1,
                "Expanding a lazy node did not invoke its loader exactly once.");

            releaseLoader.SetResult([
                new TreeNode("Alpha"),
                new TreeNode("Beta"),
            ]);

            SasdTreeNodeChildrenLoadedEventArgs loaded = await loadedEvent.Task.ConfigureAwait(true);
            Ensure(ReferenceEquals(loaded.Node, root) && loaded.ChildCount == 2,
                "Successful lazy load did not publish the expected completion event.");
            Ensure(root.Nodes.Count == 2 &&
                   root.Nodes[0].Text == "Alpha" &&
                   root.Nodes[1].Text == "Beta",
                "Successful lazy load did not replace the placeholder with loader-owned child nodes.");

            root.Collapse();
            root.Expand();
            Ensure(loaderCalls == 1,
                "A successfully loaded node called its loader again on a later expansion.");
        }
        finally
        {
            tree.NodeChildrenLoaded -= OnLoaded;
        }

        void OnLoaded(object? sender, SasdTreeNodeChildrenLoadedEventArgs args)
        {
            if (ReferenceEquals(args.Node, root))
            {
                loadedEvent.TrySetResult(args);
            }
        }
    }

    private static async Task ValidateFailureAndKeyboardRetryAsync(TestTreeView tree)
    {
        var root = new TreeNode("Restricted location");
        tree.Nodes.Add(root);

        int loaderCalls = 0;
        var failedEvent = new TaskCompletionSource<SasdTreeNodeLoadFailedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var recoveredEvent = new TaskCompletionSource<SasdTreeNodeChildrenLoadedEventArgs>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        tree.NodeLoadFailed += OnFailed;
        tree.NodeChildrenLoaded += OnLoaded;
        try
        {
            tree.RegisterLazyNode(root, _ =>
            {
                loaderCalls++;
                if (loaderCalls == 1)
                {
                    return Task.FromException<IReadOnlyList<TreeNode>>(
                        new UnauthorizedAccessException("Sensitive technical path must not be rendered."));
                }

                return Task.FromResult<IReadOnlyList<TreeNode>>([
                    new TreeNode("Recovered child"),
                ]);
            });

            root.Expand();
            SasdTreeNodeLoadFailedEventArgs failure = await failedEvent.Task.ConfigureAwait(true);

            Ensure(ReferenceEquals(failure.Node, root) && failure.Exception is UnauthorizedAccessException,
                "Lazy-load failure event did not preserve the technical exception for application diagnostics.");
            Ensure(root.Nodes.Count == 1 &&
                   root.Nodes[0].Text.Contains("retry", StringComparison.OrdinalIgnoreCase) &&
                   !root.Nodes[0].Text.Contains("Sensitive technical path", StringComparison.Ordinal),
                "TreeView exposed technical exception text instead of a generic retry action.");

            // The retry node is deliberately reachable through the TreeView's ordinary
            // keyboard event route. The smoke subclass only exposes protected WinForms event
            // dispatch; no product-only test hook or process-global SendKeys input is used.
            tree.SelectedNode = root.Nodes[0];
            KeyEventArgs enter = tree.RaiseKeyDown(Keys.Enter);
            Ensure(enter.Handled && enter.SuppressKeyPress,
                "Keyboard retry did not claim Enter while the retry node was selected.");

            SasdTreeNodeChildrenLoadedEventArgs recovered = await recoveredEvent.Task.ConfigureAwait(true);
            Ensure(ReferenceEquals(recovered.Node, root) && recovered.ChildCount == 1 && loaderCalls == 2,
                "Retry did not invoke the loader exactly once after the initial failure.");
            Ensure(root.Nodes.Count == 1 && root.Nodes[0].Text == "Recovered child",
                "Successful retry did not replace the failure node with loaded children.");
        }
        finally
        {
            tree.NodeLoadFailed -= OnFailed;
            tree.NodeChildrenLoaded -= OnLoaded;
        }

        void OnFailed(object? sender, SasdTreeNodeLoadFailedEventArgs args)
        {
            if (ReferenceEquals(args.Node, root))
            {
                failedEvent.TrySetResult(args);
            }
        }

        void OnLoaded(object? sender, SasdTreeNodeChildrenLoadedEventArgs args)
        {
            if (ReferenceEquals(args.Node, root))
            {
                recoveredEvent.TrySetResult(args);
            }
        }
    }

    private static void ValidateRegistrationGuards(TestTreeView tree)
    {
        var prepopulated = new TreeNode("Already populated");
        prepopulated.Nodes.Add(new TreeNode("Existing child"));
        tree.Nodes.Add(prepopulated);

        EnsureThrows<InvalidOperationException>(
            () => tree.RegisterLazyNode(
                prepopulated,
                _ => Task.FromResult<IReadOnlyList<TreeNode>>([])),
            "TreeView accepted lazy registration over application-owned existing children.");

        var registered = new TreeNode("Registered once");
        tree.Nodes.Add(registered);
        tree.RegisterLazyNode(registered, _ => Task.FromResult<IReadOnlyList<TreeNode>>([]));

        EnsureThrows<InvalidOperationException>(
            () => tree.RegisterLazyNode(
                registered,
                _ => Task.FromResult<IReadOnlyList<TreeNode>>([])),
            "TreeView accepted two loaders for the same node.");

        using var otherTree = new SasdTreeView();
        var foreign = new TreeNode("Foreign node");
        otherTree.Nodes.Add(foreign);
        EnsureThrows<InvalidOperationException>(
            () => tree.RegisterLazyNode(
                foreign,
                _ => Task.FromResult<IReadOnlyList<TreeNode>>([])),
            "TreeView accepted a lazy node that belongs to another TreeView.");
    }

    private static async Task ValidateCancellationOnDisposeAsync(Form hostForm)
    {
        var cancellationObserved = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var loaderFinished = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        int failures = 0;

        var tree = new TestTreeView
        {
            Dock = DockStyle.Fill,
            Visible = false,
        };
        hostForm.Controls.Add(tree);

        var root = new TreeNode("Long-running node");
        tree.Nodes.Add(root);
        tree.NodeLoadFailed += (_, _) => failures++;
        tree.RegisterLazyNode(root, async cancellationToken =>
        {
            using CancellationTokenRegistration registration = cancellationToken.Register(
                static state => ((TaskCompletionSource)state!).TrySetResult(),
                cancellationObserved);

            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
                return Array.Empty<TreeNode>();
            }
            finally
            {
                loaderFinished.TrySetResult();
            }
        });

        root.Expand();
        tree.Dispose();

        await cancellationObserved.Task.ConfigureAwait(true);
        await loaderFinished.Task.ConfigureAwait(true);
        await DrainUiQueueAsync(hostForm).ConfigureAwait(true);

        Ensure(tree.IsDisposed,
            "TreeView cancellation scenario did not dispose the component.");
        Ensure(failures == 0,
            "Disposal cancellation was surfaced as an application-visible lazy-load failure.");
    }

    private static Task DrainUiQueueAsync(Control owner)
    {
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        owner.BeginInvoke(new Action(() => completion.TrySetResult()));
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

    private sealed class TestTreeView : SasdTreeView
    {
        public KeyEventArgs RaiseKeyDown(Keys key)
        {
            var args = new KeyEventArgs(key);
            OnKeyDown(args);
            return args;
        }
    }
}
