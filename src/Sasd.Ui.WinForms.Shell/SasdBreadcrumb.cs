namespace Sasd.Ui.WinForms.Shell;

/// <summary>Represents one stable location in a breadcrumb path.</summary>
public sealed record SasdBreadcrumbItem(string Id, string Text)
{
    /// <summary>Creates a validated breadcrumb item.</summary>
    public static SasdBreadcrumbItem Create(string id, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        return new SasdBreadcrumbItem(id, text);
    }
}

/// <summary>Event data for breadcrumb navigation requested by the user.</summary>
public sealed class SasdBreadcrumbNavigateEventArgs : EventArgs
{
    /// <summary>Initialises navigation event data.</summary>
    public SasdBreadcrumbNavigateEventArgs(SasdBreadcrumbItem item) => Item = item;

    /// <summary>Gets the requested breadcrumb item.</summary>
    public SasdBreadcrumbItem Item { get; }
}

/// <summary>
/// Renders an application-owned navigation path without assuming how routes,
/// documents or domain identifiers are implemented by the consuming application.
/// </summary>
public sealed class SasdBreadcrumb : UserControl
{
    private readonly FlowLayoutPanel pathHost;
    private SasdBreadcrumbItem[] path = [];

    /// <summary>Initialises the breadcrumb control.</summary>
    public SasdBreadcrumb()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pathHost = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Margin = Padding.Empty,
        };
        Controls.Add(pathHost);
    }

    /// <summary>Raised when the user activates a navigable path segment.</summary>
    public event EventHandler<SasdBreadcrumbNavigateEventArgs>? NavigateRequested;

    /// <summary>Gets a snapshot of the current path.</summary>
    public IReadOnlyList<SasdBreadcrumbItem> Path => path;

    /// <summary>Replaces the complete breadcrumb path.</summary>
    public void SetPath(IEnumerable<SasdBreadcrumbItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        SasdBreadcrumbItem[] materialized = items.ToArray();
        if (materialized.Any(item => string.IsNullOrWhiteSpace(item.Id) || string.IsNullOrWhiteSpace(item.Text)))
        {
            throw new ArgumentException("Breadcrumb items require non-empty identifiers and text.", nameof(items));
        }

        path = materialized;
        pathHost.SuspendLayout();
        try
        {
            pathHost.Controls.Clear();
            for (int index = 0; index < materialized.Length; index++)
            {
                SasdBreadcrumbItem item = materialized[index];
                bool isCurrent = index == materialized.Length - 1;

                if (index > 0)
                {
                    pathHost.Controls.Add(new Label
                    {
                        AutoSize = true,
                        Text = "›",
                        Margin = new Padding(4, 3, 4, 0),
                        AccessibleName = "Path separator",
                    });
                }

                if (isCurrent)
                {
                    pathHost.Controls.Add(new Label
                    {
                        AutoSize = true,
                        Text = item.Text,
                        Margin = new Padding(0, 3, 0, 0),
                        AccessibleName = $"Current location: {item.Text}",
                    });
                    continue;
                }

                var link = new LinkLabel
                {
                    AutoSize = true,
                    Text = item.Text,
                    Tag = item,
                    Margin = new Padding(0, 3, 0, 0),
                    AccessibleName = $"Navigate to {item.Text}",
                };
                link.LinkClicked += OnLinkClicked;
                pathHost.Controls.Add(link);
            }
        }
        finally
        {
            pathHost.ResumeLayout(true);
        }
    }

    private void OnLinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (sender is LinkLabel { Tag: SasdBreadcrumbItem item })
        {
            NavigateRequested?.Invoke(this, new SasdBreadcrumbNavigateEventArgs(item));
        }
    }
}
