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
/// Displays a simple, keyboard-accessible breadcrumb path without owning the
/// application's navigation policy.
/// </summary>
[DefaultEvent(nameof(ItemInvoked))]
public class SasdBreadcrumb : UserControl
{
    private readonly FlowLayoutPanel host;
    private readonly List<SasdBreadcrumbItem> items = [];
    private string separatorText = "›";

    /// <summary>Initialises the breadcrumb control.</summary>
    public SasdBreadcrumb()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        AccessibleName = "Breadcrumb";

        host = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
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

    private void RebuildControls()
    {
        // Generated controls are disposed explicitly because breadcrumbs can change
        // frequently while navigating through a long-running workbench application.
        Control[] oldControls = host.Controls.Cast<Control>().ToArray();
        host.Controls.Clear();
        foreach (Control control in oldControls)
        {
            control.Dispose();
        }

        for (int index = 0; index < items.Count; index++)
        {
            SasdBreadcrumbItem item = items[index];
            bool isCurrent = index == items.Count - 1;
            host.Controls.Add(isCurrent ? CreateCurrentLabel(item) : CreateLink(item));

            if (!isCurrent)
            {
                host.Controls.Add(new Label
                {
                    AccessibleName = "Breadcrumb separator",
                    AutoSize = true,
                    Margin = new Padding(4, 4, 4, 0),
                    Text = separatorText,
                });
            }
        }
    }

    private LinkLabel CreateLink(SasdBreadcrumbItem item)
    {
        var link = new LinkLabel
        {
            AccessibleName = $"Navigate to {item.Text}",
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 0),
            TabStop = true,
            Text = item.Text,
        };
        link.LinkClicked += (_, _) => ItemInvoked?.Invoke(this, new SasdBreadcrumbItemInvokedEventArgs(item));
        return link;
    }

    private static Label CreateCurrentLabel(SasdBreadcrumbItem item) => new()
    {
        AccessibleName = $"Current location {item.Text}",
        AutoSize = true,
        Margin = new Padding(0, 4, 0, 0),
        Text = item.Text,
    };
}
