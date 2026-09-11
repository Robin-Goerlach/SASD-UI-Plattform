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
/// <remarks>
/// Views returned by registered factories become owned by this host. With
/// <see cref="CachePages"/> disabled, an inactive view is disposed when navigation leaves it.
/// With caching enabled, inactive views remain host-owned and are reused until caching is
/// disabled or the navigation host itself is disposed.
/// </remarks>
public class SasdNavigationHost : UserControl
{
    private readonly SplitContainer splitContainer;
    private readonly ListBox navigationList;
    private readonly Panel contentPanel;
    private readonly List<PageRegistration> pages = [];
    private readonly Dictionary<string, Control> cachedViews = new(StringComparer.Ordinal);
    private Control? currentView;
    private string? currentPageId;
    private bool cachePages;
    private bool internalSelectionChange;

    /// <summary>Initialises the navigation host.</summary>
    public SasdNavigationHost()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Navigation";
        AccessibleDescription = "Application navigation pages and the active page content.";

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
            AccessibleName = "Navigation pages",
            AccessibleDescription = "Selects the active application page.",
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            IntegralHeight = false,
        };
        navigationList.SelectedIndexChanged += OnSelectedIndexChanged;

        contentPanel = new Panel
        {
            AccessibleRole = AccessibleRole.Pane,
            AccessibleName = "Page content",
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            TabStop = false,
        };

        splitContainer.Panel1.Padding = new Padding(8);
        splitContainer.Panel1.Controls.Add(navigationList);
        splitContainer.Panel2.Controls.Add(contentPanel);
        Controls.Add(splitContainer);
    }

    /// <summary>Raised after a page has been activated.</summary>
    public event EventHandler<SasdNavigationEventArgs>? Navigated;

    /// <summary>Gets or sets whether created pages are retained for reuse.</summary>
    /// <remarks>
    /// Changing this property at runtime is supported. Disabling caching immediately disposes
    /// inactive cached views but leaves the currently displayed view alive until normal
    /// navigation replaces it. Enabling caching adopts the currently displayed view into the
    /// cache so it is not detached and lost on the next navigation.
    /// </remarks>
    [Category("SASD")]
    [DefaultValue(false)]
    public bool CachePages
    {
        get => cachePages;
        set
        {
            if (cachePages == value)
            {
                return;
            }

            cachePages = value;
            if (value)
            {
                CacheCurrentView();
            }
            else
            {
                ReleaseInactiveCachedViews();
            }
        }
    }

    /// <summary>Gets or sets the navigation-pane width.</summary>
    [Category("SASD")]
    [DefaultValue(220)]
    public int NavigationWidth
    {
        get => splitContainer.SplitterDistance;
        set => splitContainer.SplitterDistance = Math.Max(splitContainer.Panel1MinSize, value);
    }

    /// <summary>Registers one application-owned page factory.</summary>
    /// <remarks>
    /// The factory itself remains application-owned. A control returned successfully from the
    /// factory transfers to navigation-host ownership for the remainder of its visual lifetime.
    /// </remarks>
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
        int index = pages.FindIndex(page => string.Equals(page.Id, pageId, StringComparison.Ordinal));
        if (index < 0)
        {
            return false;
        }

        PageRegistration registration = pages[index];
        Control nextView = GetOrCreateView(registration);
        nextView.Dock = DockStyle.Fill;

        if (!ReferenceEquals(currentView, nextView))
        {
            contentPanel.Controls.Clear();
            if (currentView is not null && !CachePages)
            {
                currentView.Dispose();
            }

            currentView = nextView;
            currentPageId = registration.Id;
            contentPanel.Controls.Add(nextView);
        }
        else
        {
            // Keep identity explicit even when a cached page is reselected. This also makes
            // runtime cache-policy changes independent of ListBox selection implementation.
            currentPageId = registration.Id;
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

            // The currently displayed control belongs to contentPanel and will be disposed by
            // normal WinForms child ownership in base.Dispose(). Inactive cached pages are not
            // in that visual tree, so dispose those explicitly without double-disposing current.
            DisposeCachedViewsExcept(currentView);
            cachedViews.Clear();
        }

        base.Dispose(disposing);
    }

    private Control GetOrCreateView(PageRegistration registration)
    {
        if (CachePages && cachedViews.TryGetValue(registration.Id, out Control? cached))
        {
            if (!cached.IsDisposed)
            {
                return cached;
            }

            // The host owns cached views, so callers should not dispose them. Recover safely if
            // application code nevertheless does so; never attempt to reattach a disposed
            // WinForms control to the live content tree.
            cachedViews.Remove(registration.Id);
        }

        Control view = registration.Factory()
            ?? throw new InvalidOperationException($"Page factory '{registration.Id}' returned null.");

        if (CachePages)
        {
            cachedViews.Add(registration.Id, view);
        }

        return view;
    }

    private void CacheCurrentView()
    {
        if (currentView is null || currentView.IsDisposed || currentPageId is null)
        {
            return;
        }

        // A page created while caching was disabled is still host-owned. When caching is later
        // enabled, adopt that current instance rather than allowing the next navigation to
        // detach it without either cache ownership or disposal.
        cachedViews[currentPageId] = currentView;
    }

    private void ReleaseInactiveCachedViews()
    {
        // Turning caching off is an ownership transition, not a performance hint. Inactive
        // controls are no longer retained and must be disposed now. The current view remains a
        // normal child of contentPanel and will be disposed when navigation actually leaves it.
        DisposeCachedViewsExcept(currentView);
        cachedViews.Clear();
    }

    private void DisposeCachedViewsExcept(Control? retainedView)
    {
        var disposedViews = new HashSet<Control>();
        foreach (Control view in cachedViews.Values)
        {
            if (ReferenceEquals(view, retainedView) || !disposedViews.Add(view))
            {
                continue;
            }

            view.Dispose();
        }
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
