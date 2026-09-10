using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Manual exercise surface for smaller R1 controls that are easier to understand together than as separate pages.
/// </summary>
internal sealed class ControlsLabPage : UserControl
{
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly SasdBreadcrumb breadcrumb = new();
    private readonly SasdDocumentTabs documentTabs = new();
    private readonly SasdPager pager = new();
    private int generatedDocumentNumber = 2;

    public ControlsLabPage(Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        ConfigureBreadcrumb();
        ConfigurePager();

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(940, 0),
            Text =
                "Controls lab\r\n\r\n" +
                "This page groups several smaller R1 controls so their keyboard behaviour, events, ownership and default layout can be exercised directly. " +
                "Nothing here represents business logic; each interaction only changes this demonstration page.",
        };

        var collectionSection = new SasdSectionPanel
        {
            Dock = DockStyle.Fill,
            SectionTitle = "List and tree defaults",
        };
        collectionSection.Controls.Add(CreateCollectionSplit());

        var emptyState = new SasdEmptyState
        {
            Dock = DockStyle.Fill,
            Title = "No queued operations",
            Message = "Empty-state controls can explain why a view is empty and provide one clear next action.",
            ActionText = "Create example",
        };
        emptyState.ActionInvoked += (_, _) =>
            publishStatus("Empty-state action invoked.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(3));

        var emptySection = new SasdSectionPanel
        {
            Dock = DockStyle.Fill,
            SectionTitle = "Empty state and pager",
        };
        var emptyLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
        };
        emptyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        emptyLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        emptyLayout.Controls.Add(emptyState, 0, 0);
        emptyLayout.Controls.Add(pager, 0, 1);
        emptySection.Controls.Add(emptyLayout);

        ConfigureDocumentTabs();
        var documentSection = new SasdSectionPanel
        {
            Dock = DockStyle.Fill,
            SectionTitle = "Document tabs",
        };
        documentSection.Controls.Add(CreateDocumentArea());

        var upperSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 500,
        };
        upperSplit.Panel1.Controls.Add(collectionSection);
        upperSplit.Panel2.Controls.Add(emptySection);

        var contentSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 330,
        };
        contentSplit.Panel1.Controls.Add(upperSplit);
        contentSplit.Panel2.Controls.Add(documentSection);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(breadcrumb, 0, 1);
        layout.Controls.Add(contentSplit, 0, 2);
        Controls.Add(layout);
    }

    private void ConfigureBreadcrumb()
    {
        breadcrumb.Dock = DockStyle.Top;
        breadcrumb.SetPath(
        [
            new SasdBreadcrumbItem("home", "Showcase"),
            new SasdBreadcrumbItem("components", "Components"),
            new SasdBreadcrumbItem("lab", "Controls lab"),
        ]);
        breadcrumb.ItemInvoked += (_, args) =>
            publishStatus(
                $"Breadcrumb invoked: {args.Item.Text} ({args.Item.Id}).",
                SasdStatusSeverity.Information,
                TimeSpan.FromSeconds(3));
    }

    private void ConfigurePager()
    {
        pager.SetState(currentPageIndex: 0, currentPageSize: 10, knownTotalCount: 37);
        pager.PageRequested += (_, args) =>
        {
            // SasdPager raises intent only. The consuming application updates the state after
            // its own data operation completes; here we do that immediately because no backend exists.
            pager.SetState(args.PageIndex, args.PageSize, 37);
            publishStatus(
                $"Pager requested page {args.PageIndex + 1} with {args.PageSize} rows.",
                SasdStatusSeverity.Information,
                TimeSpan.FromSeconds(3));
        };
    }

    private Control CreateCollectionSplit()
    {
        var list = new SasdListView
        {
            Dock = DockStyle.Fill,
        };
        list.Columns.Add("Component", 170);
        list.Columns.Add("Purpose", 280);
        list.Items.Add(new ListViewItem(["SasdListView", "Business-style row selection defaults"]));
        list.Items.Add(new ListViewItem(["SasdTreeView", "Stable tree selection/rendering defaults"]));
        list.Items.Add(new ListViewItem(["SasdBreadcrumb", "Application-owned navigation path"]));
        list.Items.Add(new ListViewItem(["SasdDocumentTabs", "Owned document visual lifecycle"]));

        var tree = new SasdTreeView
        {
            Dock = DockStyle.Fill,
        };
        TreeNode forms = tree.Nodes.Add("Forms");
        forms.Nodes.Add("Field layout");
        forms.Nodes.Add("Validation");
        TreeNode data = tree.Nodes.Add("Data");
        data.Nodes.Add("Grid");
        data.Nodes.Add("Search and filters");
        data.Nodes.Add("Pager");
        TreeNode shell = tree.Nodes.Add("Shell");
        shell.Nodes.Add("Commands");
        shell.Nodes.Add("Breadcrumbs");
        shell.Nodes.Add("Documents");
        tree.ExpandAll();
        tree.AfterSelect += (_, args) =>
            publishStatus(
                $"Tree selection: {args.Node.Text}",
                SasdStatusSeverity.Information,
                TimeSpan.FromSeconds(2));

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 360,
        };
        split.Panel1.Controls.Add(list);
        split.Panel2.Controls.Add(tree);
        return split;
    }

    private void ConfigureDocumentTabs()
    {
        documentTabs.Dock = DockStyle.Fill;
        documentTabs.DocumentOpened += (_, args) =>
            publishStatus($"Opened document: {args.Title}", SasdStatusSeverity.Success, TimeSpan.FromSeconds(3));
        documentTabs.DocumentClosed += (_, args) =>
            publishStatus($"Closed document: {args.Title}", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
        documentTabs.OpenOrSelect("welcome", "Welcome", () => CreateDocumentContent(
            "Welcome document",
            "Opening the same stable document id again selects the existing visual instead of duplicating it."));
        documentTabs.OpenOrSelect("ownership", "Ownership", () => CreateDocumentContent(
            "Owned visual content",
            "Closing a document disposes its page/content. Business state that must survive should remain outside the visual control."));
    }

    private Control CreateDocumentArea()
    {
        var openButton = new Button
        {
            AutoSize = true,
            Text = "Open another document",
        };
        openButton.Click += (_, _) =>
        {
            generatedDocumentNumber++;
            string id = $"generated-{generatedDocumentNumber}";
            documentTabs.OpenOrSelect(id, $"Document {generatedDocumentNumber}", () =>
                CreateDocumentContent(
                    $"Generated document {generatedDocumentNumber}",
                    "This content is application-created and becomes owned by SasdDocumentTabs after OpenOrSelect succeeds."));
        };

        var closeButton = new Button
        {
            AutoSize = true,
            Text = "Close selected",
        };
        closeButton.Click += (_, _) =>
        {
            string? id = documentTabs.SelectedDocumentId;
            if (id is null)
            {
                publishStatus("No document is selected.", SasdStatusSeverity.Warning, TimeSpan.FromSeconds(3));
                return;
            }

            documentTabs.CloseDocument(id);
        };

        var closeAllButton = new Button
        {
            AutoSize = true,
            Text = "Close all",
        };
        closeAllButton.Click += (_, _) => documentTabs.CloseAllDocuments();

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
        };
        actions.Controls.Add(openButton);
        actions.Controls.Add(closeButton);
        actions.Controls.Add(closeAllButton);

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
        };
        panel.Controls.Add(documentTabs);
        panel.Controls.Add(actions);
        documentTabs.BringToFront();
        return panel;
    }

    private static Control CreateDocumentContent(string title, string message)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
        };
        panel.Controls.Add(new Label
        {
            AutoSize = true,
            MaximumSize = new Size(800, 0),
            Text = $"{title}\r\n\r\n{message}",
        });
        return panel;
    }
}
