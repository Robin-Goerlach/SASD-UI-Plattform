using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Event data published after a grid page has loaded.</summary>
public sealed class SasdGridPageLoadedEventArgs : EventArgs
{
    /// <summary>Initialises the event data.</summary>
    public SasdGridPageLoadedEventArgs(int pageIndex, int pageSize, int totalCount)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>Gets the zero-based page index.</summary>
    public int PageIndex { get; }

    /// <summary>Gets the page size.</summary>
    public int PageSize { get; }

    /// <summary>Gets the total number of matching records.</summary>
    public int TotalCount { get; }
}

/// <summary>
/// Event data published when a load initiated by a controller-owned WinForms event cannot
/// return its exception to an awaiting application caller.
/// </summary>
public sealed class SasdGridLoadFailedEventArgs : EventArgs
{
    /// <summary>Initialises load-failure event data.</summary>
    public SasdGridLoadFailedEventArgs(Exception exception, int pageIndex, int pageSize)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    /// <summary>Gets the data-source exception that prevented the page from loading.</summary>
    /// <remarks>
    /// This technical exception is intended for application logging/error handling. Applications
    /// should choose their own user-safe presentation rather than displaying exception text directly.
    /// </remarks>
    public Exception Exception { get; }

    /// <summary>Gets the zero-based page index requested when the failure occurred.</summary>
    public int PageIndex { get; }

    /// <summary>Gets the page size requested when the failure occurred.</summary>
    public int PageSize { get; }
}

/// <summary>
/// Coordinates a designer-friendly non-generic <see cref="SasdDataGrid"/> with a typed,
/// application-owned page source. The controller intentionally knows nothing about SQL,
/// Entity Framework, REST, SQLite, or any other concrete storage technology.
/// </summary>
/// <typeparam name="T">The row model type.</typeparam>
public sealed class SasdGridController<T> : IDisposable
{
    private readonly SasdDataGrid grid;
    private readonly ISasdDataPageSource<T> source;
    private readonly BindingSource bindingSource = new();
    private CancellationTokenSource? activeLoad;
    private SasdPager? pager;
    private bool disposed;

    /// <summary>Initialises the controller.</summary>
    public SasdGridController(SasdDataGrid grid, ISasdDataPageSource<T> source, int pageSize = 25)
    {
        this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        PageSize = pageSize;
        grid.DataSource = bindingSource;
        grid.ColumnHeaderMouseClick += OnColumnHeaderMouseClick;
    }

    /// <summary>Raised after a page has loaded successfully.</summary>
    public event EventHandler<SasdGridPageLoadedEventArgs>? PageLoaded;

    /// <summary>
    /// Raised when paging or header sorting starts a load from a WinForms event and that load fails.
    /// </summary>
    /// <remarks>
    /// Explicit calls to <see cref="LoadAsync"/>, <see cref="SearchAsync"/> and <see cref="SortAsync"/>
    /// continue to propagate failures through their returned <see cref="Task"/>. This event exists for
    /// controller-owned <c>async void</c> WinForms event paths, where no application Task caller exists.
    /// Cancellation caused by a newer request or controller disposal is expected and is not reported as
    /// a failure.
    /// </remarks>
    public event EventHandler<SasdGridLoadFailedEventArgs>? LoadFailed;

    /// <summary>Gets the current zero-based page index.</summary>
    public int PageIndex { get; private set; }

    /// <summary>Gets the current page size.</summary>
    public int PageSize { get; private set; }

    /// <summary>Gets the current search text supplied to the data source.</summary>
    public string? SearchText { get; private set; }

    /// <summary>Gets the active sort descriptors.</summary>
    public IReadOnlyList<SasdSortDescriptor> Sort { get; private set; } = Array.Empty<SasdSortDescriptor>();

    /// <summary>Gets whether an asynchronous page load is in progress.</summary>
    public bool IsLoading => activeLoad is not null;

    /// <summary>Connects a pager to the controller.</summary>
    public void AttachPager(SasdPager value)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(value);
        if (ReferenceEquals(pager, value))
        {
            return;
        }

        // Detach first so replacing a pager can never leave duplicate event handlers behind.
        if (pager is not null)
        {
            pager.PageRequested -= OnPageRequested;
        }

        pager = value;
        pager.PageRequested += OnPageRequested;
    }

    /// <summary>Loads the current page using the current search and sort state.</summary>
    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        // A new request supersedes an older request. Cancel the older operation, but do not
        // dispose its CancellationTokenSource here: the older LoadAsync still owns and may be
        // using that source through a CancellationToken. Each invocation disposes its own CTS
        // in its finally block after the application-owned page source has actually returned.
        activeLoad?.Cancel();
        var load = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        activeLoad = load;

        try
        {
            SasdDataPage<T> page = await LoadPageAsync(load.Token).ConfigureAwait(true);

            bindingSource.DataSource = new BindingList<T>(page.Items.ToList());
            pager?.SetState(PageIndex, PageSize, page.TotalCount);
            PageLoaded?.Invoke(this, new SasdGridPageLoadedEventArgs(PageIndex, PageSize, page.TotalCount));
        }
        finally
        {
            // The local invocation owns its linked CTS even after a newer request replaces
            // activeLoad. Clearing the shared marker is conditional; disposal of the local
            // resource is not.
            if (ReferenceEquals(activeLoad, load))
            {
                activeLoad = null;
            }

            load.Dispose();
        }
    }

    /// <summary>Changes the search text, resets paging, and reloads.</summary>
    public Task SearchAsync(string? searchText, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        SearchText = string.IsNullOrWhiteSpace(searchText) ? null : searchText.Trim();
        PageIndex = 0;
        return LoadAsync(cancellationToken);
    }

    /// <summary>Changes the active sort and reloads from the first page.</summary>
    public Task SortAsync(
        string field,
        SasdSortDirection direction,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        Sort = [SasdSortDescriptor.Create(field, direction)];
        PageIndex = 0;
        return LoadAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        grid.ColumnHeaderMouseClick -= OnColumnHeaderMouseClick;
        if (pager is not null)
        {
            pager.PageRequested -= OnPageRequested;
        }

        // LoadAsync owns disposal of its linked CTS. The controller only requests
        // cancellation here so a still-running application data source can finish its own
        // cancellation/registration cleanup before that CTS is released.
        activeLoad?.Cancel();
        activeLoad = null;
        bindingSource.Dispose();
    }

    private async Task<SasdDataPage<T>> LoadPageAsync(CancellationToken cancellationToken)
    {
        SasdDataQuery query = CreateQuery();

        // Continue on the WinForms synchronization context because BindingSource and pager
        // updates performed by the caller must run on the control owner thread.
        SasdDataPage<T> page = await source.LoadAsync(query, cancellationToken).ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();

        int maximumPageIndex = GetMaximumPageIndex(page.TotalCount);
        if (PageIndex <= maximumPageIndex)
        {
            return page;
        }

        // A common CRUD case is deleting the last row(s) on the final page and then refreshing.
        // The data source can correctly report a total count whose final valid page is now below
        // the requested index. Realign the controller itself (not just the visual pager) and
        // perform one bounded corrective load so the user sees the new final page immediately.
        PageIndex = maximumPageIndex;
        SasdDataQuery correctedQuery = CreateQuery();
        page = await source.LoadAsync(correctedQuery, cancellationToken).ConfigureAwait(true);
        cancellationToken.ThrowIfCancellationRequested();

        int correctedMaximumPageIndex = GetMaximumPageIndex(page.TotalCount);
        if (PageIndex > correctedMaximumPageIndex)
        {
            // The source changed again while the corrective request was in flight. Avoid an
            // unbounded retry loop and, more importantly, never label records from one offset as
            // belonging to another page. Clamp state and expose a safe empty page; a later reload
            // can repopulate the newly valid page once the source stabilizes.
            PageIndex = correctedMaximumPageIndex;
            return SasdDataPage<T>.Create(Array.Empty<T>(), page.TotalCount);
        }

        return page;
    }

    private SasdDataQuery CreateQuery() =>
        new(
            checked(PageIndex * PageSize),
            PageSize,
            Sort,
            SearchText).Validate();

    private int GetMaximumPageIndex(int totalCount) =>
        totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)PageSize) - 1;

    private async void OnPageRequested(object? sender, SasdPageRequestedEventArgs e)
    {
        PageIndex = e.PageIndex;
        PageSize = e.PageSize;
        try
        {
            await LoadAsync().ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer page request or controller disposal superseded this one.
        }
        catch (Exception exception)
        {
            PublishEventLoadFailure(exception);
        }
    }

    private async void OnColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex < 0 || e.ColumnIndex >= grid.Columns.Count)
        {
            return;
        }

        var column = grid.Columns[e.ColumnIndex];
        string? field = string.IsNullOrWhiteSpace(column.DataPropertyName) ? column.Name : column.DataPropertyName;
        if (string.IsNullOrWhiteSpace(field))
        {
            return;
        }

        SasdSortDescriptor? currentSort = Sort.Count > 0 ? Sort[0] : null;
        var direction = currentSort?.Field == field && currentSort.Direction == SasdSortDirection.Ascending
            ? SasdSortDirection.Descending
            : SasdSortDirection.Ascending;

        try
        {
            await SortAsync(field, direction).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer sort/page request or controller disposal superseded this one.
        }
        catch (Exception exception)
        {
            PublishEventLoadFailure(exception);
        }
    }

    private void PublishEventLoadFailure(Exception exception)
    {
        if (disposed)
        {
            return;
        }

        LoadFailed?.Invoke(this, new SasdGridLoadFailedEventArgs(exception, PageIndex, PageSize));
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);
}
