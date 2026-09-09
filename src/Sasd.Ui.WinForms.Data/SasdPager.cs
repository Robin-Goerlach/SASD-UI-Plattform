using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Event data for a page request.</summary>
public sealed class SasdPageRequestedEventArgs : EventArgs
{
    /// <summary>Initialises page-request event data.</summary>
    public SasdPageRequestedEventArgs(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    /// <summary>Gets the zero-based page index.</summary>
    public int PageIndex { get; }

    /// <summary>Gets the requested page size.</summary>
    public int PageSize { get; }
}

/// <summary>
/// Provides conservative paging controls for business data views. The control only
/// raises page requests; loading data remains the responsibility of a controller or
/// consuming application.
/// </summary>
public class SasdPager : UserControl
{
    private readonly Button previousButton;
    private readonly Button nextButton;
    private readonly Label pageLabel;
    private readonly ComboBox pageSizeComboBox;
    private int pageIndex;
    private int pageSize = 25;
    private int totalCount;
    private bool internalChange;

    /// <summary>Initialises the pager.</summary>
    public SasdPager()
    {
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var layout = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };

        previousButton = new Button { AutoSize = true, Text = "Previous" };
        nextButton = new Button { AutoSize = true, Text = "Next" };
        pageLabel = new Label { AutoSize = true, Margin = new Padding(10, 7, 10, 0) };
        pageSizeComboBox = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 72,
        };
        pageSizeComboBox.Items.AddRange([10, 25, 50, 100]);
        pageSizeComboBox.SelectedItem = pageSize;

        previousButton.Click += (_, _) => RequestPage(Math.Max(0, pageIndex - 1));
        nextButton.Click += (_, _) => RequestPage(pageIndex + 1);
        pageSizeComboBox.SelectedIndexChanged += OnPageSizeChanged;

        layout.Controls.Add(previousButton);
        layout.Controls.Add(pageLabel);
        layout.Controls.Add(nextButton);
        layout.Controls.Add(new Label { AutoSize = true, Margin = new Padding(18, 7, 6, 0), Text = "Rows" });
        layout.Controls.Add(pageSizeComboBox);
        Controls.Add(layout);
        UpdateState();
    }

    /// <summary>Raised when the user requests another page or page size.</summary>
    public event EventHandler<SasdPageRequestedEventArgs>? PageRequested;

    /// <summary>Gets the current zero-based page index.</summary>
    [Browsable(false)]
    public int PageIndex => pageIndex;

    /// <summary>Gets the current page size.</summary>
    [Browsable(false)]
    public int PageSize => pageSize;

    /// <summary>Gets the known total row count.</summary>
    [Browsable(false)]
    public int TotalCount => totalCount;

    /// <summary>Updates the pager after a data page has been loaded.</summary>
    public void SetState(int currentPageIndex, int currentPageSize, int knownTotalCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(currentPageIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(currentPageSize);
        ArgumentOutOfRangeException.ThrowIfNegative(knownTotalCount);

        internalChange = true;
        try
        {
            pageSize = currentPageSize;
            totalCount = knownTotalCount;
            int maximumPage = Math.Max(0, PageCount - 1);
            pageIndex = Math.Min(currentPageIndex, maximumPage);

            // Preserve an application-specific page size even if it is not one of
            // the standard values offered by the control.
            if (!pageSizeComboBox.Items.Contains(pageSize))
            {
                pageSizeComboBox.Items.Add(pageSize);
            }

            pageSizeComboBox.SelectedItem = pageSize;
            UpdateState();
        }
        finally
        {
            internalChange = false;
        }
    }

    private int PageCount => totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);

    private void OnPageSizeChanged(object? sender, EventArgs e)
    {
        if (internalChange || pageSizeComboBox.SelectedItem is not int selectedPageSize)
        {
            return;
        }

        pageSize = selectedPageSize;
        RequestPage(0);
    }

    private void RequestPage(int requestedPageIndex)
    {
        int maximumPage = Math.Max(0, PageCount - 1);
        int validPageIndex = Math.Clamp(requestedPageIndex, 0, maximumPage);
        PageRequested?.Invoke(this, new SasdPageRequestedEventArgs(validPageIndex, pageSize));
    }

    private void UpdateState()
    {
        int first = totalCount == 0 ? 0 : (pageIndex * pageSize) + 1;
        int last = totalCount == 0 ? 0 : Math.Min(totalCount, (pageIndex + 1) * pageSize);
        pageLabel.Text = $"{first}–{last} of {totalCount}";
        previousButton.Enabled = pageIndex > 0;
        nextButton.Enabled = pageIndex < PageCount - 1;
    }
}
