using System.ComponentModel;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>Represents one location in a breadcrumb path.</summary>
public sealed record SasdBreadcrumbItem(string Id, string Text)
{
    /// <summary>Creates and validates a breadcrumb item.</summary>
    public static SasdBreadcrumbItem Create(string id, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        return new SasdBreadcrumbItem(id, text);
    }
}

/// <summary>Event data for an invoked breadcrumb item.</summary>
public sealed class SasdBreadcrumbItemInvokedEventArgs : EventArgs
{
    /// <summary>Initialises the event data.</summary>
    public SasdBreadcrumbItemInvokedEventArgs(SasdBreadcrumbItem item) => Item = item;

    /// <summary>Gets the selected breadcrumb item.</summary>
    public SasdBreadcrumbItem Item { get; }
}

/// <summary>
/// Displays a keyboard-accessible breadcrumb path without owning the application's
/// navigation policy.
/// </summary>
[DefaultEvent(nameof(ItemInvoked))]
public class SasdBreadcrumb : UserControl
{
    private const int DefaultMaximumVisibleItems = 5;

    private readonly FlowLayoutPanel host;
    private readonly List<SasdBreadcrumbItem> items = [];
    private string separatorText = "›";
    private int maximumVisibleItems = DefaultMaximumVisibleItems;
    private ContextMenuStrip? overflowMenu;
    private int collapsedItemCount;

    /// <summary>Initialises the breadcrumb control.</summary>
    public SasdBreadcrumb()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Breadcrumb";
        AccessibleDescription = "No breadcrumb locations.";

        host = new FlowLayoutPanel
        {
            AccessibleRole = AccessibleRole.Grouping,
            AccessibleName = "Breadcrumb path",
            AccessibleDescription = "Navigation path from the root to the current location.",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            TabStop = false,
            WrapContents = false,
        };
        Controls.Add(host);
    }

    /// <summary>Occurs when the user invokes a breadcrumb item before the current location.</summary>
    public event EventHandler<SasdBreadcrumbItemInvokedEventArgs>? ItemInvoked;

    /// <summary>Gets a snapshot of the current path.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<SasdBreadcrumbItem> Items => items.ToArray();

    /// <summary>Gets the number of breadcrumb items.</summary>
    [Browsable(false)]
    public int ItemCount => items.Count;

    /// <summary>
    /// Gets or sets the maximum number of real breadcrumb locations shown before intermediate
    /// locations are represented by an ellipsis menu.
    /// </summary>
    /// <remarks>
    /// The root and current location are always retained. The ellipsis itself is an additional
    /// presentation control and does not count toward this value. Count-based collapsing keeps
    /// the R1 behavior deterministic across fonts, themes and DPI settings; applications that
    /// later need pixel-perfect adaptive layout can build that policy above this stable contract.
    /// </remarks>
    [Category("SASD")]
    [DefaultValue(DefaultMaximumVisibleItems)]
    public int MaximumVisibleItems
    {
        get => maximumVisibleItems;
        set
        {
            if (value < 2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    "At least the root and current breadcrumb locations must remain visible.");
            }

            if (maximumVisibleItems == value)
            {
                return;
            }

            maximumVisibleItems = value;
            RebuildControls();
        }
    }

    /// <summary>Gets or sets the visual separator between breadcrumb items.</summary>
    [Category("SASD")]
    [DefaultValue("›")]
    public string SeparatorText
    {
        get => separatorText;
        set
        {
            string next = string.IsNullOrWhiteSpace(value) ? "›" : value;
            if (string.Equals(separatorText, next, StringComparison.Ordinal))
            {
                return;
            }

            separatorText = next;
            RebuildControls();
        }
    }

    /// <summary>Replaces the entire breadcrumb path.</summary>
    public void SetPath(IEnumerable<SasdBreadcrumbItem> path)
    {
        ArgumentNullException.ThrowIfNull(path);

        var validated = new List<SasdBreadcrumbItem>();
        var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (SasdBreadcrumbItem item in path)
        {
            ArgumentNullException.ThrowIfNull(item);
            SasdBreadcrumbItem checkedItem = SasdBreadcrumbItem.Create(item.Id, item.Text);
            if (!ids.Add(checkedItem.Id))
            {
                throw new ArgumentException($"Breadcrumb id '{checkedItem.Id}' occurs more than once.", nameof(path));
            }

            validated.Add(checkedItem);
        }

        items.Clear();
        items.AddRange(validated);
        RebuildControls();
    }

    /// <summary>Clears the breadcrumb path.</summary>
    public void ClearPath()
    {
        if (items.Count == 0)
        {
            return;
        }

        items.Clear();
        RebuildControls();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // ContextMenuStrip is not part of the Controls hierarchy, so the breadcrumb must
            // release this generated native resource explicitly. Child path controls remain
            // normal WinForms children and are disposed by the base control hierarchy.
            overflowMenu?.Dispose();
            overflowMenu = null;
        }

        base.Dispose(disposing);
    }

    private void RebuildControls()
    {
        // Generated controls are owned by the breadcrumb. Disposing them explicitly is
        // important because navigation can rebuild this path many times during one long-
        // running workbench session; merely detaching the old controls would retain their
        // managed/native resources until some unrelated later collection.
        Control[] oldControls = host.Controls.Cast<Control>().ToArray();
        host.Controls.Clear();
        foreach (Control control in oldControls)
        {
            control.Dispose();
        }

        // The overflow menu is not parented into host.Controls and therefore requires its own
        // lifecycle. Rebuilding the path invalidates every captured breadcrumb item in it.
        overflowMenu?.Dispose();
        overflowMenu = null;
        collapsedItemCount = 0;

        if (items.Count <= maximumVisibleItems)
        {
            for (int index = 0; index < items.Count; index++)
            {
                AddPathItem(items[index], isCurrent: index == items.Count - 1);
                AddSeparatorAfter(index, items.Count);
            }
        }
        else
        {
            // Keep the root plus the newest tail. Intermediate ancestors remain reachable
            // through one explicit ellipsis menu rather than being silently discarded.
            AddPathItem(items[0], isCurrent: false);
            AddSeparator();

            int tailItemCount = maximumVisibleItems - 1;
            int tailStartIndex = items.Count - tailItemCount;
            SasdBreadcrumbItem[] hiddenItems = items
                .Skip(1)
                .Take(tailStartIndex - 1)
                .ToArray();

            collapsedItemCount = hiddenItems.Length;
            host.Controls.Add(CreateOverflowLink(hiddenItems));
            AddSeparator();

            for (int index = tailStartIndex; index < items.Count; index++)
            {
                AddPathItem(items[index], isCurrent: index == items.Count - 1);
                AddSeparatorAfter(index, items.Count);
            }
        }

        UpdateAccessibleState();
    }

    private void AddPathItem(SasdBreadcrumbItem item, bool isCurrent) =>
        host.Controls.Add(isCurrent ? CreateCurrentLabel(item) : CreateLink(item));

    private void AddSeparatorAfter(int index, int itemCount)
    {
        if (index < itemCount - 1)
        {
            AddSeparator();
        }
    }

    private void AddSeparator() => host.Controls.Add(new Label
    {
        AccessibleRole = AccessibleRole.Separator,
        AutoSize = true,
        Margin = new Padding(4, 4, 4, 0),
        TabStop = false,
        Text = separatorText,
    });

    private LinkLabel CreateLink(SasdBreadcrumbItem item)
    {
        var link = new LinkLabel
        {
            AccessibleRole = AccessibleRole.Link,
            AccessibleName = $"Navigate to {item.Text}",
            AccessibleDescription = $"Navigates to the breadcrumb location {item.Text}.",
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 0),
            TabStop = true,
            Text = item.Text,
        };
        link.LinkClicked += (_, _) => RaiseItemInvoked(item);
        return link;
    }

    private LinkLabel CreateOverflowLink(IReadOnlyList<SasdBreadcrumbItem> hiddenItems)
    {
        overflowMenu = new ContextMenuStrip
        {
            AccessibleName = "Hidden breadcrumb locations",
        };

        foreach (SasdBreadcrumbItem item in hiddenItems)
        {
            var menuItem = new ToolStripMenuItem(item.Text)
            {
                AccessibleName = $"Navigate to {item.Text}",
                AccessibleDescription = $"Navigates to the hidden breadcrumb location {item.Text}.",
            };
            menuItem.Click += (_, _) => RaiseItemInvoked(item);
            overflowMenu.Items.Add(menuItem);
        }

        var overflowLink = new LinkLabel
        {
            AccessibleRole = AccessibleRole.Link,
            AccessibleName = $"Show {hiddenItems.Count} hidden breadcrumb locations",
            AccessibleDescription = "Opens the intermediate breadcrumb locations that were collapsed to keep the path compact.",
            AutoSize = true,
            ContextMenuStrip = overflowMenu,
            Margin = new Padding(0, 4, 0, 0),
            TabStop = true,
            Text = "…",
        };
        overflowLink.LinkClicked += (_, _) => ShowOverflowMenu(overflowLink);
        return overflowLink;
    }

    private void ShowOverflowMenu(LinkLabel overflowLink)
    {
        ContextMenuStrip? menu = overflowMenu;
        if (menu is null || menu.IsDisposed || menu.Items.Count == 0 || overflowLink.IsDisposed)
        {
            return;
        }

        // LinkClicked is raised by the native LinkLabel for pointer and keyboard activation.
        // Keeping menu display here means the platform does not need process-global input hooks
        // or application routing knowledge to make collapsed ancestors reachable.
        menu.Show(overflowLink, new Point(0, overflowLink.Height));
    }

    private void RaiseItemInvoked(SasdBreadcrumbItem item) =>
        ItemInvoked?.Invoke(this, new SasdBreadcrumbItemInvokedEventArgs(item));

    private static Label CreateCurrentLabel(SasdBreadcrumbItem item) => new()
    {
        AccessibleRole = AccessibleRole.StaticText,
        AccessibleName = $"Current location {item.Text}",
        AccessibleDescription = "Current breadcrumb location.",
        AutoSize = true,
        Margin = new Padding(0, 4, 0, 0),
        TabStop = false,
        Text = item.Text,
    };

    private void UpdateAccessibleState()
    {
        if (items.Count == 0)
        {
            AccessibleDescription = "No breadcrumb locations.";
            return;
        }

        string noun = items.Count == 1 ? "location" : "locations";
        string collapsed = collapsedItemCount == 0
            ? string.Empty
            : $" {collapsedItemCount} intermediate {(collapsedItemCount == 1 ? "location is" : "locations are")} collapsed under the ellipsis.";
        AccessibleDescription =
            $"{items.Count} breadcrumb {noun}. Current location: {items[^1].Text}.{collapsed}";
    }
}
