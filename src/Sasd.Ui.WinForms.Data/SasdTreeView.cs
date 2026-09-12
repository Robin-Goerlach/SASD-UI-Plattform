using System.Runtime.CompilerServices;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Event data for a successfully loaded lazy tree node.</summary>
public sealed class SasdTreeNodeChildrenLoadedEventArgs : EventArgs
{
    /// <summary>Initialises the event data.</summary>
    public SasdTreeNodeChildrenLoadedEventArgs(TreeNode node, int childCount)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        ChildCount = childCount;
    }

    /// <summary>Gets the application-owned parent node whose children were loaded.</summary>
    public TreeNode Node { get; }

    /// <summary>Gets the number of child nodes returned by the loader.</summary>
    public int ChildCount { get; }
}

/// <summary>Event data for a lazy tree-node load that could not be completed.</summary>
public sealed class SasdTreeNodeLoadFailedEventArgs : EventArgs
{
    /// <summary>Initialises the event data.</summary>
    public SasdTreeNodeLoadFailedEventArgs(TreeNode node, Exception exception)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }

    /// <summary>Gets the application-owned parent node whose child load failed.</summary>
    public TreeNode Node { get; }

    /// <summary>
    /// Gets the technical exception for application logging/diagnostics.
    /// </summary>
    /// <remarks>
    /// The exception is deliberately not copied into the visible retry node. Applications
    /// should choose their own user-safe diagnostics instead of exposing raw exception text.
    /// </remarks>
    public Exception Exception { get; }
}

/// <summary>
/// TreeView with stable selection/rendering defaults and bounded lazy-child loading for SASD
/// navigation and data trees.
/// </summary>
/// <remarks>
/// Lazy loading remains application-owned: the platform controls when a registered loader is
/// invoked and how loading/failure/retry state is presented, but it does not know about file
/// systems, databases, APIs or domain objects. Child nodes returned by a loader transfer to the
/// normal WinForms tree ownership hierarchy when they are attached.
/// </remarks>
public class SasdTreeView : TreeView
{
    private const string LoadingText = "Loading...";
    private const string RetryText = "Unable to load items. Press Enter or double-click to retry.";

    private readonly ConditionalWeakTable<TreeNode, LazyNodeState> lazyNodes = new();
    private readonly HashSet<CancellationTokenSource> activeLoads = [];

    /// <summary>Initialises the tree view.</summary>
    public SasdTreeView()
    {
        AccessibleRole = AccessibleRole.Outline;
        AccessibleName = "Tree";
        DoubleBuffered = true;
        HideSelection = false;
        ShowNodeToolTips = true;

        BeforeExpand += HandleBeforeExpand;
        NodeMouseDoubleClick += HandleNodeMouseDoubleClick;
        KeyDown += HandleKeyDown;
    }

    /// <summary>Occurs after a registered lazy node successfully receives its child nodes.</summary>
    public event EventHandler<SasdTreeNodeChildrenLoadedEventArgs>? NodeChildrenLoaded;

    /// <summary>
    /// Occurs when a registered lazy-node loader fails during expansion or a retry action.
    /// </summary>
    /// <remarks>
    /// Event-driven loading cannot return a <see cref="Task"/> to the application. The event
    /// therefore carries the technical exception so applications can log or diagnose it while
    /// the control presents only a generic, retryable user-facing node.
    /// </remarks>
    public event EventHandler<SasdTreeNodeLoadFailedEventArgs>? NodeLoadFailed;

    /// <summary>
    /// Registers an unloaded node whose children should be obtained on first expansion.
    /// </summary>
    /// <param name="node">The application-owned parent node to make lazy.</param>
    /// <param name="loader">
    /// Application-owned asynchronous loader. Returned child nodes must be detached from any
    /// existing parent/tree; they become part of this tree when loading succeeds.
    /// </param>
    /// <remarks>
    /// Register the node before adding ordinary child nodes. A lightweight placeholder is added
    /// so native WinForms displays an expansion affordance before the real children are known.
    /// The loader may perform I/O but must honor the supplied cancellation token during normal
    /// shutdown whenever practical.
    /// </remarks>
    public void RegisterLazyNode(
        TreeNode node,
        Func<CancellationToken, Task<IReadOnlyList<TreeNode>>> loader)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(loader);

        if (node.TreeView is not null && !ReferenceEquals(node.TreeView, this))
        {
            throw new InvalidOperationException("The lazy node already belongs to a different TreeView.");
        }

        if (lazyNodes.TryGetValue(node, out _))
        {
            throw new InvalidOperationException("The tree node already has a lazy-child loader.");
        }

        if (node.Nodes.Count != 0)
        {
            throw new InvalidOperationException(
                "Register a lazy node before attaching application child nodes; the platform will add its loading placeholder.");
        }

        lazyNodes.Add(node, new LazyNodeState(loader));
        node.Nodes.Add(CreateLoadingNode());
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            BeforeExpand -= HandleBeforeExpand;
            NodeMouseDoubleClick -= HandleNodeMouseDoubleClick;
            KeyDown -= HandleKeyDown;

            // Do not dispose these cancellation sources here. The application-owned loader may
            // still be unwinding and legitimately using its token. Each load owns and disposes
            // its source in its own finally block after that loader task has actually completed.
            foreach (CancellationTokenSource cancellation in activeLoads.ToArray())
            {
                cancellation.Cancel();
            }
        }

        base.Dispose(disposing);
    }

    private async void HandleBeforeExpand(object? sender, TreeViewCancelEventArgs e)
    {
        if (!lazyNodes.TryGetValue(e.Node, out LazyNodeState? state) || state.IsLoaded || state.IsLoading)
        {
            return;
        }

        await LoadChildrenFromUiEventAsync(e.Node, state).ConfigureAwait(true);
    }

    private async void HandleNodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node is RetryTreeNode retryNode)
        {
            await RetryFromUiEventAsync(retryNode.ParentNode).ConfigureAwait(true);
        }
    }

    private async void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || SelectedNode is not RetryTreeNode retryNode)
        {
            return;
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
        await RetryFromUiEventAsync(retryNode.ParentNode).ConfigureAwait(true);
    }

    private async Task RetryFromUiEventAsync(TreeNode parentNode)
    {
        if (!lazyNodes.TryGetValue(parentNode, out LazyNodeState? state) || state.IsLoading)
        {
            return;
        }

        state.IsLoaded = false;
        await LoadChildrenFromUiEventAsync(parentNode, state).ConfigureAwait(true);
    }

    private async Task LoadChildrenFromUiEventAsync(TreeNode node, LazyNodeState state)
    {
        if (IsDisposed || Disposing || state.IsLoaded || state.IsLoading)
        {
            return;
        }

        state.IsLoading = true;
        ReplaceChildren(node, CreateLoadingNode());

        var cancellation = new CancellationTokenSource();
        activeLoads.Add(cancellation);

        try
        {
            IReadOnlyList<TreeNode> loadedChildren = await state.Loader(cancellation.Token).ConfigureAwait(true)
                ?? throw new InvalidOperationException("The lazy tree-node loader returned null.");

            cancellation.Token.ThrowIfCancellationRequested();
            if (IsDisposed || Disposing)
            {
                return;
            }

            TreeNode[] children = ValidateLoadedChildren(node, loadedChildren);
            node.Nodes.Clear();
            node.Nodes.AddRange(children);
            state.IsLoaded = true;
            NodeChildrenLoaded?.Invoke(this, new SasdTreeNodeChildrenLoadedEventArgs(node, children.Length));
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            // Disposal is an expected lifecycle boundary. Do not replace the visual tree or
            // publish a failure after the owner has requested cancellation during shutdown.
        }
        catch (Exception exception)
        {
            if (!IsDisposed && !Disposing)
            {
                ReplaceChildren(node, new RetryTreeNode(node));
                NodeLoadFailed?.Invoke(this, new SasdTreeNodeLoadFailedEventArgs(node, exception));
            }
        }
        finally
        {
            state.IsLoading = false;
            activeLoads.Remove(cancellation);
            cancellation.Dispose();
        }
    }

    private static TreeNode[] ValidateLoadedChildren(TreeNode parentNode, IReadOnlyList<TreeNode> loadedChildren)
    {
        var uniqueNodes = new HashSet<TreeNode>();
        var children = new TreeNode[loadedChildren.Count];

        for (int index = 0; index < loadedChildren.Count; index++)
        {
            TreeNode child = loadedChildren[index]
                ?? throw new InvalidOperationException("The lazy tree-node loader returned a null child node.");

            if (ReferenceEquals(child, parentNode))
            {
                throw new InvalidOperationException("A lazy tree-node loader cannot return its parent node as a child.");
            }

            if (child.Parent is not null || child.TreeView is not null)
            {
                throw new InvalidOperationException(
                    "Lazy tree-node loaders must return detached child nodes that are not already attached to another tree location.");
            }

            if (!uniqueNodes.Add(child))
            {
                throw new InvalidOperationException("A lazy tree-node loader returned the same child node more than once.");
            }

            children[index] = child;
        }

        return children;
    }

    private static void ReplaceChildren(TreeNode parentNode, TreeNode replacement)
    {
        parentNode.Nodes.Clear();
        parentNode.Nodes.Add(replacement);
    }

    private static TreeNode CreateLoadingNode() => new(LoadingText)
    {
        ToolTipText = "Children are being loaded.",
    };

    private sealed class LazyNodeState(Func<CancellationToken, Task<IReadOnlyList<TreeNode>>> loader)
    {
        public Func<CancellationToken, Task<IReadOnlyList<TreeNode>>> Loader { get; } = loader;

        public bool IsLoaded { get; set; }

        public bool IsLoading { get; set; }
    }

    private sealed class RetryTreeNode(TreeNode parentNode) : TreeNode(RetryText)
    {
        public TreeNode ParentNode { get; } = parentNode;
    }
}
