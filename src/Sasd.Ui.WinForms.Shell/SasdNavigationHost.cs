using System.ComponentModel;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>Event data for a completed navigation.</summary>
public sealed class SasdNavigationEventArgs : EventArgs
{
    /// <summary>Initialises navigation event data.</summary>
    public SasdNavigationEventArgs(string pageId) => PageId = pageId;

    /// <summary>Gets the stable page identifier.</summary>
    public string PageId { get; }
}

/// <summary>
/// Provides a small list/content shell for CRUD, utility and workbench applications.
/// Applications own page factories and business navigation policy.
/// </summary>
public class SasdNavigationHost : UserControl
{
    private readonly SplitContainer splitContainer;
    private readonly ListBox navigationList;
    private readonly Panel contentPanel;
    private readonly List<PageRegistration> pages = [];
    private readonly Dictionary<string, Control> cachedViews = new(StringComparer.Ordinal);
    private Control? currentView;
    private bool internalSelectionChange;

    /// <summary>Initialises the navigation host.</summary>
    public SasdNavigationHost()
    {
        splitContainer = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            IsSplitterFixed = false,
            Panel1MinSize = 140,
            SplitterDistance = 220,
            SplitterWidth = 4,
        };

        navigationList = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            IntegralHeight = false,
        };
        navigationList.SelectedIndexChanged += OnSelectedIndexChanged;

        contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
        };

        splitContainer.Panel1.Padding = new Padding(8);
        splitContainer.Panel1.Controls.Add(navigationList);
        splitContainer.Panel2.Controls.Add(contentPanel);
        Controls.Add(splitContainer);
    }

    /// <summary>Raised after a page has been activated.</summary>
    public event EventHandler<SasdNavigationEventArgs>? Navigated;

    /// <summary>Gets or sets whether created pages are retained for reuse.</summary>
    [Category("SASD")]
    [DefaultValue(false)]
    public bool CachePages { get; set; }

    /// <summary>Gets or sets the navigation-pane width.</summary>
    [Category("SASD")]
    [DefaultValue(220)]
    public int NavigationWidth
    {
        get => splitContainer.SplitterDistance;
        set => splitContainer.SplitterDistance = Math.Max(splitContainer.Panel1MinSize, value);
    }

    /// <summary>Registers one application-owned page factory.</summary>
    public void RegisterPage(string id, string title, Func<Control> factory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(factory);

        if (pages.Any(page => string.Equals(page.Id, id, StringComparison.Ordinal)))
        {
            throw new InvalidOperationException($"A page with ID '{id}' is already registered.");
        }

        pages.Add(new PageRegistration(id, title, factory));
        navigationList.Items.Add(title);
    }

    /// <summary>Activates a registered page by its stable identifier.</summary>
    public bool Navigate(string pageId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pageId);
        var index = pages.FindIndex(page => string.Equals(page.Id, pageId, StringComparison.Ordinal));
        if (index < 0)
        {
            return false;
        }

        var registration = pages[index];
        var nextView = GetOrCreateView(registration);
        nextView.Dock = DockStyle.Fill;

        if (!ReferenceEquals(currentView, nextView))
        {
            contentPanel.Controls.Clear();
            if (currentView is not null && !CachePages)
            {
                currentView.Dispose();
            }

            currentView = nextView;
            contentPanel.Controls.Add(nextView);
        }

        internalSelectionChange = true;
        try
        {
            navigationList.SelectedIndex = index;
        }
        finally
        {
            internalSelectionChange = false;
        }

        Navigated?.Invoke(this, new SasdNavigationEventArgs(pageId));
        return true;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            navigationList.SelectedIndexChanged -= OnSelectedIndexChanged;
            foreach (var view in cachedViews.Values)
            {
                view.Dispose();
            }

            cachedViews.Clear();
        }

        base.Dispose(disposing);
    }

    private Control GetOrCreateView(PageRegistration registration)
    {
        if (CachePages && cachedViews.TryGetValue(registration.Id, out var cached))
        {
            return cached;
        }

        var view = registration.Factory()
            ?? throw new InvalidOperationException($"Page factory '{registration.Id}' returned null.");

        if (CachePages)
        {
            cachedViews.Add(registration.Id, view);
        }

        return view;
    }

    private void OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (internalSelectionChange || navigationList.SelectedIndex < 0)
        {
            return;
        }

        Navigate(pages[navigationList.SelectedIndex].Id);
    }

    private sealed record PageRegistration(string Id, string Title, Func<Control> Factory);
}
