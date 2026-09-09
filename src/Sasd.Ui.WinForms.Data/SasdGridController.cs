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
/// Coordinates a designer-friendly non-generic <see cref="SasdDataGrid"/> with a typed, application-owned page source.
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
        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

        PageSize = pageSize;
        grid.DataSource = bindingSource;
        grid.ColumnHeaderMouseClick += OnColumnHeaderMouseClick;
    }

    /// <summary>Raised after a page has loaded successfully.</summary>
    public event EventHandler<SasdGridPageLoadedEventArgs>? PageLoaded;

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
        activeLoad?.Cancel();
        activeLoad?.Dispose();
        activeLoad = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var load = activeLoad;

        try
        {
            var query = new SasdDataQuery(
                checked(PageIndex * PageSize),
                PageSize,
                Sort,
                SearchText).Validate();

            var page = await source.LoadAsync(query, load.Token).ConfigureAwait(true);
            load.Token.ThrowIfCancellationRequested();

            bindingSource.DataSource = new BindingList<T>(page.Items.ToList());
            pager?.SetState(PageIndex, PageSize, page.TotalCount);
            PageLoaded?.Invoke(this, new SasdGridPageLoadedEventArgs(PageIndex, PageSize, page.TotalCount));
        }
        finally
        {
            if (ReferenceEquals(activeLoad, load))
            {
                activeLoad.Dispose();
                activeLoad = null;
            }
        }
    }

    /// <summary>Changes the search text, resets paging, and reloads.</summary>
    public Task SearchAsync(string? searchText, CancellationToken cancellationToken = default)
    {
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

        activeLoad?.Cancel();
        activeLoad?.Dispose();
        bindingSource.Dispose();
    }

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
            // A newer page request superseded this one.
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

        var direction = Sort.FirstOrDefault()?.Field == field && Sort.First().Direction == SasdSortDirection.Ascending
            ? SasdSortDirection.Descending
            : SasdSortDirection.Ascending;

        try
        {
            await SortAsync(field, direction).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer sort or page request superseded this one.
        }
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);
}
