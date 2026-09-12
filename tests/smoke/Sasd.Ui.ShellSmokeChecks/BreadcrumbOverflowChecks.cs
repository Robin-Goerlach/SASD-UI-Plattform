using System.Reflection;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.ShellSmokeChecks;

/// <summary>
/// Focused evidence for the count-bounded breadcrumb ellipsis contract. These checks stay in
/// the existing Shell smoke subsystem because overflow is part of navigation presentation,
/// not a separate component family.
/// </summary>
internal static class BreadcrumbOverflowChecks
{
    public static void Run()
    {
        using var form = new Form
        {
            ClientSize = new Size(720, 160),
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD breadcrumb overflow smoke",
        };
        var breadcrumb = new SasdBreadcrumb { Dock = DockStyle.Top };
        form.Controls.Add(breadcrumb);
        form.Show();

        Ensure(breadcrumb.MaximumVisibleItems == 5,
            "Breadcrumb default maximum-visible-item contract changed unexpectedly.");

        SasdBreadcrumbItem[] longPath =
        [
            SasdBreadcrumbItem.Create("root", "Home"),
            SasdBreadcrumbItem.Create("customers", "Customers"),
            SasdBreadcrumbItem.Create("region", "EMEA"),
            SasdBreadcrumbItem.Create("account", "Example AG"),
            SasdBreadcrumbItem.Create("contracts", "Contracts"),
            SasdBreadcrumbItem.Create("year", "2026"),
            SasdBreadcrumbItem.Create("contract", "Contract 7"),
        ];
        breadcrumb.SetPath(longPath);

        FlowLayoutPanel host = breadcrumb.Controls.OfType<FlowLayoutPanel>().Single();
        LinkLabel overflowLink = host.Controls
            .OfType<LinkLabel>()
            .Single(link => string.Equals(link.Text, "…", StringComparison.Ordinal));
        ContextMenuStrip firstMenu = GetOverflowMenu(breadcrumb)
            ?? throw new InvalidOperationException("Long breadcrumb path did not create its overflow menu.");

        Ensure(overflowLink.TabStop && overflowLink.AccessibleRole == AccessibleRole.Link,
            "Breadcrumb ellipsis is not reachable as a normal accessible link.");
        Ensure(overflowLink.AccessibleName == "Show 2 hidden breadcrumb locations",
            "Breadcrumb ellipsis does not describe the number of hidden locations.");
        Ensure(firstMenu.Items.Count == 2 &&
               firstMenu.Items[0].Text == "Customers" &&
               firstMenu.Items[1].Text == "EMEA",
            "Breadcrumb overflow menu does not contain the expected intermediate ancestors in path order.");
        Ensure(breadcrumb.AccessibleDescription?.Contains(
                   "2 intermediate locations are collapsed under the ellipsis.",
                   StringComparison.Ordinal) == true,
            "Breadcrumb group accessibility does not announce collapsed intermediate locations.");

        SasdBreadcrumbItem? invoked = null;
        int invocations = 0;
        breadcrumb.ItemInvoked += (_, args) =>
        {
            invocations++;
            invoked = args.Item;
        };

        ((ToolStripMenuItem)firstMenu.Items[1]).PerformClick();
        Ensure(invocations == 1 && invoked?.Id == "region",
            "Invoking a hidden breadcrumb menu item did not reuse the normal stable ItemInvoked route.");

        Control[] firstPresentation = host.Controls.Cast<Control>().ToArray();
        breadcrumb.MaximumVisibleItems = 3;
        ContextMenuStrip secondMenu = GetOverflowMenu(breadcrumb)
            ?? throw new InvalidOperationException("Tighter breadcrumb limit did not rebuild its overflow menu.");

        Ensure(firstPresentation.All(static control => control.IsDisposed),
            "Changing the breadcrumb overflow limit detached old generated controls without disposing them.");
        Ensure(firstMenu.IsDisposed,
            "Changing the breadcrumb overflow limit left the previous ContextMenuStrip undisposed.");
        Ensure(secondMenu.Items.Count == 4,
            "Tighter breadcrumb limit did not move the expected number of ancestors into overflow.");
        Ensure(breadcrumb.AccessibleDescription?.Contains(
                   "4 intermediate locations are collapsed under the ellipsis.",
                   StringComparison.Ordinal) == true,
            "Breadcrumb accessible state did not refresh after overflow-limit change.");

        EnsureThrows<ArgumentOutOfRangeException>(
            () => breadcrumb.MaximumVisibleItems = 1,
            "Breadcrumb accepted an overflow limit that cannot preserve root and current locations.");

        breadcrumb.SetPath([
            SasdBreadcrumbItem.Create("root", "Home"),
            SasdBreadcrumbItem.Create("current", "Current"),
        ]);
        Ensure(secondMenu.IsDisposed,
            "Replacing a long path with a short path did not dispose the overflow menu.");
        Ensure(GetOverflowMenu(breadcrumb) is null &&
               host.Controls.OfType<LinkLabel>().All(link => link.Text != "…"),
            "Short breadcrumb path retained an unnecessary ellipsis presentation.");

        breadcrumb.MaximumVisibleItems = 2;
        breadcrumb.SetPath(longPath);
        ContextMenuStrip finalMenu = GetOverflowMenu(breadcrumb)
            ?? throw new InvalidOperationException("Long path did not rebuild overflow before disposal check.");
        breadcrumb.Dispose();
        Ensure(finalMenu.IsDisposed,
            "Disposing the breadcrumb did not release its non-child overflow menu resource.");

        form.Hide();
    }

    private static ContextMenuStrip? GetOverflowMenu(SasdBreadcrumb breadcrumb)
    {
        FieldInfo? field = typeof(SasdBreadcrumb).GetField(
            "overflowMenu",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (field is null)
        {
            throw new InvalidOperationException("Could not inspect breadcrumb overflow ownership state.");
        }

        return field.GetValue(breadcrumb) as ContextMenuStrip;
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
}
