using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.Sample.Workbench;

/// <summary>
/// Reference workbench application that composes the reusable SASD shell, command,
/// navigation, tree and document-tab components into a realistic desktop workflow.
/// </summary>
/// <remarks>
/// <para>
/// The sample deliberately uses in-memory application data and a normal multiline
/// <see cref="TextBox"/> as its editor. Rich editor and docking packages are R2
/// adapter decisions and are not required to prove the R1 workbench composition.
/// </para>
/// <para>
/// Document identity/content remain application-owned. <see cref="SasdDocumentTabs"/>
/// owns only the visual controls created for an open document and disposes those
/// controls when a tab closes.
/// </para>
/// </remarks>
internal sealed class WorkbenchSampleForm : SasdShellForm
{
    private const string WorkspacePageId = "workspace";
    private const string AboutPageId = "about";

    private readonly List<WorkbenchDocument> documents = CreateInitialDocuments();
    private readonly SasdCommand newNoteCommand;
    private readonly SasdCommand openSelectedCommand;
    private readonly SasdCommand closeDocumentCommand;
    private readonly SasdCommand closeAllCommand;

    private SasdTreeView? documentTree;
    private SasdDocumentTabs? documentTabs;
    private SasdBreadcrumb? breadcrumb;
    private bool workspaceActive;
    private int nextNoteNumber = 1;

    /// <summary>Initialises the in-memory Workbench reference application.</summary>
    public WorkbenchSampleForm()
    {
        Text = "SASD UI Platform — Workbench Reference";
        StateKey = "Sample.Workbench.Main";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);
        ClientSize = new Size(1180, 760);

        // A workbench normally keeps its primary page alive while users switch to
        // auxiliary pages. CachePages gives the application that lifecycle without
        // moving business state into the navigation component itself.
        NavigationHost.CachePages = true;

        newNoteCommand = new SasdCommand(
            "workbench.new-note",
            "New note",
            _ =>
            {
                CreateNewNote();
                return Task.CompletedTask;
            },
            "Create an editable in-memory note.",
            Keys.Control | Keys.N);

        openSelectedCommand = new SasdCommand(
            "workbench.open-selected",
            "Open",
            _ =>
            {
                OpenSelectedTreeDocument();
                return Task.CompletedTask;
            },
            "Open the selected tree document.",
            Keys.Control | Keys.O)
        {
            Enabled = false,
        };

        closeDocumentCommand = new SasdCommand(
            "workbench.close-document",
            "Close",
            _ =>
            {
                CloseSelectedDocument();
                return Task.CompletedTask;
            },
            "Close the active document tab.",
            Keys.Control | Keys.W)
        {
            Enabled = false,
        };

        closeAllCommand = new SasdCommand(
            "workbench.close-all",
            "Close all",
            _ =>
            {
                CloseAllDocuments();
                return Task.CompletedTask;
            },
            "Close every open document tab.",
            Keys.Control | Keys.Shift | Keys.W)
        {
            Enabled = false,
        };

        RegisterCommand(newNoteCommand);
        RegisterCommand(openSelectedCommand);
        CommandBar.AddSeparator();
        RegisterCommand(closeDocumentCommand);
        RegisterCommand(closeAllCommand);

        NavigationHost.RegisterPage(WorkspacePageId, "Workspace", CreateWorkspacePage);
        NavigationHost.RegisterPage(AboutPageId, "About", CreateAboutPage);
        NavigationHost.Navigated += OnNavigated;

        NavigationHost.Navigate(WorkspacePageId);
        PublishStatus("Workbench reference application ready.", SasdStatusSeverity.Success);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigationHost.Navigated -= OnNavigated;

            if (documentTree is not null)
            {
                documentTree.AfterSelect -= OnTreeAfterSelect;
                documentTree.NodeMouseDoubleClick -= OnTreeNodeMouseDoubleClick;
                documentTree.KeyDown -= OnTreeKeyDown;
            }

            if (documentTabs is not null)
            {
                documentTabs.DocumentOpened -= OnDocumentOpened;
                documentTabs.DocumentSelected -= OnDocumentSelected;
                documentTabs.DocumentClosed -= OnDocumentClosed;
            }

            if (breadcrumb is not null)
            {
                breadcrumb.ItemInvoked -= OnBreadcrumbItemInvoked;
            }
        }

        base.Dispose(disposing);
    }

    private SplitContainer CreateWorkspacePage()
    {
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            Panel1MinSize = 200,
            Panel2MinSize = 420,
            SplitterDistance = 280,
        };

        documentTree = new SasdTreeView
        {
            AccessibleName = "Workbench documents",
            Dock = DockStyle.Fill,
        };
        documentTree.AfterSelect += OnTreeAfterSelect;
        documentTree.NodeMouseDoubleClick += OnTreeNodeMouseDoubleClick;
        documentTree.KeyDown += OnTreeKeyDown;
        PopulateDocumentTree(documentTree);
        split.Panel1.Padding = new Padding(8);
        split.Panel1.Controls.Add(documentTree);

        breadcrumb = new SasdBreadcrumb
        {
            Dock = DockStyle.Fill,
        };
        breadcrumb.ItemInvoked += OnBreadcrumbItemInvoked;
        breadcrumb.SetPath([SasdBreadcrumbItem.Create(WorkspacePageId, "Workspace")]);

        documentTabs = new SasdDocumentTabs
        {
            Dock = DockStyle.Fill,
        };
        documentTabs.DocumentOpened += OnDocumentOpened;
        documentTabs.DocumentSelected += OnDocumentSelected;
        documentTabs.DocumentClosed += OnDocumentClosed;

        var documentLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            RowCount = 2,
        };
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        documentLayout.Controls.Add(breadcrumb, 0, 0);
        documentLayout.Controls.Add(documentTabs, 0, 1);

        split.Panel2.Padding = new Padding(8);
        split.Panel2.Controls.Add(documentLayout);

        // Open one deterministic document so a freshly launched sample immediately
        // demonstrates the document host rather than presenting an unexplained blank area.
        WorkbenchDocument first = documents[0];
        SelectTreeDocument(first.Id);
        OpenDocument(first);
        return split;
    }

    private static TableLayoutPanel CreateAboutPage()
    {
        var title = new Label
        {
            AutoSize = true,
            Text = "Workbench reference application",
        };

        var description = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(760, 0),
            Text =
                "This sample demonstrates application-owned documents composed with the SASD shell, " +
                "commands, global shortcuts, tree navigation, breadcrumbs, document tabs and status service. " +
                "It intentionally uses no docking or rich-editor dependency.",
        };

        var shortcuts = new Label
        {
            AutoSize = true,
            Text = "Shortcuts: Ctrl+N new note · Ctrl+O open selected · Ctrl+W close · Ctrl+Shift+W close all",
        };

        var layout = new TableLayoutPanel
        {
            AutoScroll = true,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            RowCount = 4,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(description, 0, 1);
        layout.Controls.Add(shortcuts, 0, 2);
        return layout;
    }

    private void PopulateDocumentTree(SasdTreeView tree)
    {
        tree.BeginUpdate();
        try
        {
            tree.Nodes.Clear();

            var projectRoot = new TreeNode("Project documents")
            {
                Name = "project-documents",
                ToolTipText = "Read-only example project documents",
            };
            var notesRoot = new TreeNode("Notes")
            {
                Name = "notes",
                ToolTipText = "Editable in-memory notes",
            };

            foreach (WorkbenchDocument document in documents)
            {
                TreeNode node = CreateDocumentNode(document);
                (document.IsEditable ? notesRoot : projectRoot).Nodes.Add(node);
            }

            tree.Nodes.Add(projectRoot);
            tree.Nodes.Add(notesRoot);
            projectRoot.Expand();
            notesRoot.Expand();
        }
        finally
        {
            tree.EndUpdate();
        }
    }

    private static TreeNode CreateDocumentNode(WorkbenchDocument document) => new(document.Title)
    {
        Name = document.Id,
        Tag = document,
        ToolTipText = document.IsEditable
            ? "Editable application-owned note"
            : "Read-only application-owned project document",
    };

    private void CreateNewNote()
    {
        if (!workspaceActive || documentTree is null)
        {
            return;
        }

        int number = nextNoteNumber++;
        var document = new WorkbenchDocument(
            $"note-{number}",
            $"Note {number}",
            string.Empty,
            isEditable: true);
        documents.Add(document);

        TreeNode? notesRoot = documentTree.Nodes.Cast<TreeNode>()
            .FirstOrDefault(node => string.Equals(node.Name, "notes", StringComparison.Ordinal));
        if (notesRoot is null)
        {
            // The tree is application-owned sample state. If its expected root was
            // removed by future sample changes, rebuild rather than silently losing the note.
            PopulateDocumentTree(documentTree);
            notesRoot = documentTree.Nodes["notes"]
                ?? throw new InvalidOperationException("The Workbench Notes root could not be rebuilt.");
        }

        TreeNode newNode = CreateDocumentNode(document);
        notesRoot.Nodes.Add(newNode);
        notesRoot.Expand();
        documentTree.SelectedNode = newNode;
        newNode.EnsureVisible();
        OpenDocument(document);
        PublishStatus($"Created {document.Title}.", SasdStatusSeverity.Success);
    }

    private void OpenSelectedTreeDocument()
    {
        if (!workspaceActive || documentTree?.SelectedNode?.Tag is not WorkbenchDocument document)
        {
            return;
        }

        OpenDocument(document);
    }

    private void OpenDocument(WorkbenchDocument document)
    {
        SasdDocumentTabs tabs = documentTabs
            ?? throw new InvalidOperationException("The Workbench document host has not been created yet.");

        tabs.OpenOrSelect(
            document.Id,
            document.Title,
            () => CreateDocumentEditor(document));
        PublishStatus($"Opened {document.Title}.");
    }

    private static TableLayoutPanel CreateDocumentEditor(WorkbenchDocument document)
    {
        var modeLabel = new Label
        {
            AutoSize = true,
            Text = document.IsEditable
                ? "Editable in-memory note — changes update the sample model immediately."
                : "Read-only project document — the application owns this content.",
        };

        var editor = new TextBox
        {
            AcceptsReturn = true,
            AcceptsTab = true,
            AccessibleName = $"Editor for {document.Title}",
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = !document.IsEditable,
            ScrollBars = ScrollBars.Both,
            Text = document.Content,
            WordWrap = false,
        };

        if (document.IsEditable)
        {
            // The TextBox and the document are both owned by this sample. The handler
            // is stored on the TextBox itself, so disposing the tab also releases the
            // delegate graph without a process-lifetime subscription.
            editor.TextChanged += (_, _) => document.Content = editor.Text;
        }

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = new Padding(4),
            RowCount = 2,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(modeLabel, 0, 0);
        layout.Controls.Add(editor, 0, 1);
        return layout;
    }

    private void CloseSelectedDocument()
    {
        if (!workspaceActive || documentTabs?.SelectedDocumentId is not string documentId)
        {
            return;
        }

        documentTabs.CloseDocument(documentId);
    }

    private void CloseAllDocuments()
    {
        if (!workspaceActive || documentTabs is null)
        {
            return;
        }

        int count = documentTabs.DocumentCount;
        documentTabs.CloseAllDocuments();
        if (count > 0)
        {
            PublishStatus($"Closed {count} document(s).");
        }
    }

    private void OnNavigated(object? sender, SasdNavigationEventArgs e)
    {
        workspaceActive = string.Equals(e.PageId, WorkspacePageId, StringComparison.Ordinal);
        UpdateCommandStates();
        PublishStatus(workspaceActive ? "Workspace active." : "About page active.");
    }

    private void OnTreeAfterSelect(object? sender, TreeViewEventArgs e) => UpdateCommandStates();

    private void OnTreeNodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node.Tag is WorkbenchDocument document)
        {
            OpenDocument(document);
        }
    }

    private void OnTreeKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || documentTree?.SelectedNode?.Tag is not WorkbenchDocument document)
        {
            return;
        }

        OpenDocument(document);
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void OnDocumentOpened(object? sender, SasdDocumentEventArgs e) => UpdateCommandStates();

    private void OnDocumentSelected(object? sender, SasdDocumentEventArgs e)
    {
        UpdateBreadcrumb(e.DocumentId, e.Title);
        UpdateCommandStates();
    }

    private void OnDocumentClosed(object? sender, SasdDocumentEventArgs e)
    {
        if (documentTabs?.SelectedDocumentId is null)
        {
            breadcrumb?.SetPath([SasdBreadcrumbItem.Create(WorkspacePageId, "Workspace")]);
        }

        UpdateCommandStates();
        PublishStatus($"Closed {e.Title}.");
    }

    private void OnBreadcrumbItemInvoked(object? sender, SasdBreadcrumbItemInvokedEventArgs e)
    {
        if (!string.Equals(e.Item.Id, WorkspacePageId, StringComparison.Ordinal))
        {
            return;
        }

        documentTree?.Focus();
    }

    private void UpdateBreadcrumb(string documentId, string title)
    {
        breadcrumb?.SetPath([
            SasdBreadcrumbItem.Create(WorkspacePageId, "Workspace"),
            SasdBreadcrumbItem.Create(documentId, title),
        ]);
    }

    private void UpdateCommandStates()
    {
        bool hasTreeDocument = workspaceActive && documentTree?.SelectedNode?.Tag is WorkbenchDocument;
        bool hasSelectedTab = workspaceActive && documentTabs?.SelectedDocumentId is not null;
        bool hasAnyTabs = workspaceActive && documentTabs is { DocumentCount: > 0 };

        newNoteCommand.Enabled = workspaceActive;
        openSelectedCommand.Enabled = hasTreeDocument;
        closeDocumentCommand.Enabled = hasSelectedTab;
        closeAllCommand.Enabled = hasAnyTabs;
    }

    private void SelectTreeDocument(string documentId)
    {
        if (documentTree is null)
        {
            return;
        }

        foreach (TreeNode root in documentTree.Nodes)
        {
            TreeNode? match = root.Nodes.Cast<TreeNode>()
                .FirstOrDefault(node => string.Equals(node.Name, documentId, StringComparison.Ordinal));
            if (match is null)
            {
                continue;
            }

            documentTree.SelectedNode = match;
            match.EnsureVisible();
            return;
        }
    }

    private void PublishStatus(string text, SasdStatusSeverity severity = SasdStatusSeverity.Information) =>
        StatusService.Publish(new SasdStatusMessage(text, severity, Priority: 0));

    private static List<WorkbenchDocument> CreateInitialDocuments() =>
    [
        new WorkbenchDocument(
            "readme",
            "README.md",
            "# Example Workbench\r\n\r\nThis document is application-owned and displayed through SasdDocumentTabs.",
            isEditable: false),
        new WorkbenchDocument(
            "architecture",
            "Architecture notes",
            "Shell -> Workspace -> Tree + Breadcrumb + Document Tabs\r\n\r\nBusiness state remains outside visual controls.",
            isEditable: false),
        new WorkbenchDocument(
            "scratch-note",
            "Scratch note",
            "Edit this note. Closing and reopening recreates the editor from the application-owned document model.",
            isEditable: true),
    ];
}
