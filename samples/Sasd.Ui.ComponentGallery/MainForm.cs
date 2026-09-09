using Sasd.Ui.Core;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;
using Sasd.Ui.WinForms.Theming;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.ComponentGallery;

/// <summary>
/// Executable specification for the reusable native WinForms foundation.
/// Each page demonstrates component behaviour that consuming SASD applications can adopt directly.
/// </summary>
internal sealed class MainForm : SasdForm
{
    private static readonly string[] CustomerCategories = ["Customer", "Partner", "Internal"];

    private readonly SasdThemeService themeService = new();
    private readonly SasdDialogService dialogService = new();
    private readonly SasdFileDialogService fileDialogService = new();
    private readonly SasdCommandRunner commandRunner = new();
    private readonly SasdStatusBar statusBar = new();
    private readonly SasdDataGrid customerGrid = new();
    private readonly IReadOnlyList<CustomerRow> customers = CreateCustomers();
    private readonly List<SasdCommandBinding> commandBindings = [];
    private readonly SasdStateStore stateStore;
    private readonly SasdFormStateService formStateService;
    private SasdValidationCoordinator? validationCoordinator;

    public MainForm()
    {
        stateStore = new SasdStateStore(new SasdStateStoreOptions("SASD-GmbH", "Sasd.Ui.ComponentGallery"));
        formStateService = new SasdFormStateService(stateStore);

        StateKey = "ComponentGallery.MainWindow";
        Text = "SASD UI Platform — Component Gallery";
        MinimumSize = new Size(960, 640);
        ClientSize = new Size(1240, 780);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(CreateRootLayout());
        Shown += OnShown;
        FormClosing += OnFormClosing;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            validationCoordinator?.Dispose();
            foreach (var binding in commandBindings)
            {
                binding.Dispose();
            }

            stateStore.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        base.Dispose(disposing);
    }

    private TableLayoutPanel CreateRootLayout()
    {
        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(16),
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(CreateTabs(), 0, 1);

        statusBar.Dock = DockStyle.Fill;
        statusBar.ShowMessage(new SasdStatusMessage("Ready — native WinForms foundation", Priority: 0));
        root.Controls.Add(statusBar, 0, 2);
        return root;
    }

    private TableLayoutPanel CreateHeader()
    {
        var header = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 12),
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        var titlePanel = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
        };
        Font titlePrototype = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
        titlePanel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font(titlePrototype.FontFamily, 20, FontStyle.Bold),
            Text = "SASD UI Platform",
        });
        titlePanel.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "Component Gallery — executable examples for reusable WinForms foundations",
        });

        var themePicker = new ComboBox
        {
            AccessibleName = "Theme",
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 150,
        };
        themePicker.Items.AddRange(["Light", "Dark", "High Contrast"]);
        themePicker.SelectedIndex = 0;
        themePicker.SelectedIndexChanged += (_, _) => ApplySelectedTheme(themePicker.SelectedIndex);

        var pickerPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 6, 0, 0),
        };
        pickerPanel.Controls.Add(new Label { AutoSize = true, Margin = new Padding(0, 7, 8, 0), Text = "Theme" });
        pickerPanel.Controls.Add(themePicker);

        header.Controls.Add(titlePanel, 0, 0);
        header.Controls.Add(pickerPanel, 1, 0);
        return header;
    }

    private TabControl CreateTabs()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(CreateOverviewPage());
        tabs.TabPages.Add(CreateFormsPage());
        tabs.TabPages.Add(CreateDataPage());
        tabs.TabPages.Add(CreateCommandsPage());
        tabs.TabPages.Add(CreateShellPage());
        tabs.TabPages.Add(CreateFeedbackPage());
        return tabs;
    }

    private static TabPage CreateOverviewPage()
    {
        var page = new TabPage("Overview") { Padding = new Padding(16) };
        var section = new SasdSectionPanel
        {
            Dock = DockStyle.Top,
            Height = 265,
            SectionTitle = "Usable native foundation",
        };
        section.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            Text = "This Gallery now exercises reusable SASD components rather than repository placeholders:\r\n\r\n" +
                   "• Light/Dark/High Contrast design tokens and native theme mapping\r\n" +
                   "• form sections, field layout and validation coordination\r\n" +
                   "• search, grid layout state, paging contracts and CSV export\r\n" +
                   "• commands shared across multiple WinForms surfaces\r\n" +
                   "• navigation and prioritized status messages\r\n" +
                   "• recoverable JSON UI state and safe window restoration\r\n" +
                   "• file/folder, clipboard and constrained shell integration\r\n" +
                   "• standard feedback, error-detail and busy-state components\r\n\r\n" +
                   "Krypton remains isolated for the visual comparison step after the native baseline is stable.",
        });
        page.Controls.Add(section);
        return page;
    }

    private TabPage CreateFormsPage()
    {
        var page = new TabPage("Forms") { Padding = new Padding(16) };
        var section = new SasdSectionPanel
        {
            Dock = DockStyle.Top,
            Height = 390,
            SectionTitle = "Field layout and validation",
        };

        var fields = new SasdFieldLayout { Dock = DockStyle.Top, Padding = new Padding(12) };
        var nameTextBox = new TextBox { Text = "Example AG" };
        var emailTextBox = new TextBox { Text = "contact@example.test" };
        fields.AddField("Name", nameTextBox, required: true);
        fields.AddField("E-mail", emailTextBox, required: true);
        fields.AddField("Category", new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = CustomerCategories,
        });

        validationCoordinator = new SasdValidationCoordinator(this);
        validationCoordinator.AddRequired("name", nameTextBox, "Name");
        validationCoordinator.AddRequired("email", emailTextBox, "E-mail");
        validationCoordinator.AddRule("email", emailTextBox, _ => ValueTask.FromResult<SasdValidationMessage?>(
            emailTextBox.Text.Contains('@', StringComparison.Ordinal)
                ? null
                : new SasdValidationMessage("email", "E-mail must contain an @ character.")));

        var summary = new SasdValidationSummary { Dock = DockStyle.Top };
        int summaryRow = fields.RowCount;
        fields.RowCount++;
        fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fields.Controls.Add(summary, 0, summaryRow);
        fields.SetColumnSpan(summary, 2);

        var validateButton = new Button { AutoSize = true, Text = "Validate sample" };
        validateButton.Click += async (_, _) =>
        {
            var result = await validationCoordinator.ValidateAsync().ConfigureAwait(true);
            summary.ShowResult(result);
            ShowStatus(result.IsValid ? "Form validation succeeded" : "Form validation found errors",
                result.IsValid ? SasdStatusSeverity.Success : SasdStatusSeverity.Warning,
                result.IsValid ? 1 : 5,
                TimeSpan.FromSeconds(4));
        };

        int actionRow = fields.RowCount;
        fields.RowCount++;
        fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fields.Controls.Add(validateButton, 1, actionRow);

        section.Controls.Add(fields);
        page.Controls.Add(section);
        return page;
    }

    private TabPage CreateDataPage()
    {
        var page = new TabPage("Data") { Padding = new Padding(16) };
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var search = new SasdSearchBox
        {
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 8),
            PlaceholderText = "Search customers...",
        };
        search.SearchTextChanged += (_, _) => FilterCustomers(search.SearchText);

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0, 0, 0, 8),
        };
        actions.Controls.Add(CreateActionButton("Export CSV", ExportCustomersAsync));
        actions.Controls.Add(CreateActionButton("Save layout", SaveGridLayoutAsync));
        actions.Controls.Add(CreateActionButton("Restore layout", RestoreGridLayoutAsync));

        customerGrid.Dock = DockStyle.Fill;
        customerGrid.ReadOnly = true;
        customerGrid.DataSource = customers.ToList();

        layout.Controls.Add(search, 0, 0);
        layout.Controls.Add(actions, 0, 1);
        layout.Controls.Add(customerGrid, 0, 2);
        page.Controls.Add(layout);
        return page;
    }

    private TabPage CreateCommandsPage()
    {
        var page = new TabPage("Commands") { Padding = new Padding(16) };
        var section = new SasdSectionPanel
        {
            Dock = DockStyle.Top,
            Height = 245,
            SectionTitle = "One command, multiple surfaces",
        };

        var content = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(12),
            WrapContents = false,
        };

        var refreshCommand = new SasdCommand(
            "gallery.refresh-sample",
            "Refresh sample",
            async cancellationToken =>
            {
                ShowStatus("Refresh command running…", SasdStatusSeverity.Information, 2);
                await Task.Delay(350, cancellationToken).ConfigureAwait(true);
                ShowStatus("Refresh command completed", SasdStatusSeverity.Success, 2, TimeSpan.FromSeconds(3));
            },
            "Demonstrates shared command state and re-entry protection.",
            Keys.Control | Keys.R);

        var button = new Button { AutoSize = true, MinimumSize = new Size(170, 32) };
        AddCommandBinding(SasdCommandBinding.Bind(button, refreshCommand, commandRunner));
        content.Controls.Add(button);

        var toolStrip = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, AutoSize = true };
        var toolButton = new ToolStripButton();
        toolStrip.Items.Add(toolButton);
        AddCommandBinding(SasdCommandBinding.Bind(toolButton, refreshCommand, commandRunner));
        content.Controls.Add(toolStrip);

        var enabledCheckBox = new CheckBox
        {
            AutoSize = true,
            Checked = true,
            Text = "Command enabled",
        };
        enabledCheckBox.CheckedChanged += (_, _) => refreshCommand.Enabled = enabledCheckBox.Checked;
        content.Controls.Add(enabledCheckBox);
        content.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "Shortcut metadata: Ctrl+R. Global shortcut routing is deliberately a later shell step.",
        });

        section.Controls.Add(content);
        page.Controls.Add(section);
        return page;
    }

    private TabPage CreateShellPage()
    {
        var page = new TabPage("Shell") { Padding = new Padding(16) };
        var host = new SasdNavigationHost
        {
            CachePages = true,
            Dock = DockStyle.Fill,
            NavigationWidth = 190,
        };
        host.RegisterPage("summary", "Summary", () => CreateShellSample("Summary", "A lightweight page registry drives the content region."));
        host.RegisterPage("settings", "Settings", () => CreateShellSample("Settings", "Applications keep ownership of page contents and business navigation policy."));
        host.Navigated += (_, e) => ShowStatus($"Navigated to: {e.PageId}", Lifetime: TimeSpan.FromSeconds(2));
        host.Navigate("summary");
        page.Controls.Add(host);
        return page;
    }

    private TabPage CreateFeedbackPage()
    {
        var page = new TabPage("Feedback") { Padding = new Padding(16) };
        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            RowCount = 1,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));

        var rightHost = new Panel { Dock = DockStyle.Fill };
        var empty = new SasdEmptyState
        {
            Dock = DockStyle.Fill,
            Title = "No notifications",
            Message = "This is the standard empty state for a feature that currently contains no items.",
            ActionText = "Create example",
        };
        empty.ActionInvoked += (_, _) => ShowStatus("Empty-state action invoked", Lifetime: TimeSpan.FromSeconds(3));

        var busy = new SasdBusyOverlay { Dock = DockStyle.Fill };
        rightHost.Controls.Add(empty);
        rightHost.Controls.Add(busy);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(0, 8, 16, 0),
            WrapContents = false,
        };
        buttons.Controls.Add(CreateDialogButton("Information", () =>
            dialogService.ShowInformation(this, "Reusable information dialog.", "Information")));
        buttons.Controls.Add(CreateDialogButton("Warning", () =>
            dialogService.ShowWarning(this, "Reusable warning dialog.", "Warning")));
        buttons.Controls.Add(CreateDialogButton("Detailed error", () =>
            dialogService.ShowErrorDetails(
                this,
                "The operation could not be completed. The user-facing message remains concise.",
                "Example error",
                "Example technical details\r\nError code: GALLERY-DEMO\r\nNo sensitive data should be placed here.")));
        buttons.Controls.Add(CreateDialogButton("Confirmation", () =>
        {
            bool confirmed = dialogService.Confirm(this, "Continue with the example action?", "Confirm");
            ShowStatus(confirmed ? "Confirmation accepted" : "Confirmation cancelled", Lifetime: TimeSpan.FromSeconds(3));
        }));
        buttons.Controls.Add(CreateActionButton("Busy for 1 second", async () =>
        {
            busy.BeginBusy("Simulating background work…");
            try
            {
                await Task.Delay(1000).ConfigureAwait(true);
            }
            finally
            {
                busy.EndBusy();
            }
        }));

        layout.Controls.Add(buttons, 0, 0);
        layout.Controls.Add(rightHost, 1, 0);
        page.Controls.Add(layout);
        return page;
    }

    private static Control CreateShellSample(string title, string description)
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
        var label = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(620, 0),
            Text = $"{title}\r\n\r\n{description}",
        };
        panel.Controls.Add(label);
        return panel;
    }

    private static Button CreateDialogButton(string text, Action action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 8),
            MinimumSize = new Size(180, 32),
            Text = text,
        };
        button.Click += (_, _) => action();
        return button;
    }

    private static Button CreateActionButton(string text, Func<Task> action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 8, 8),
            Text = text,
        };
        button.Click += async (_, _) => await action().ConfigureAwait(true);
        return button;
    }

    private void AddCommandBinding(SasdCommandBinding binding)
    {
        binding.CommandCompleted += (_, e) =>
        {
            if (!e.Result.Succeeded)
            {
                ShowStatus(e.Result.UserMessage ?? "Command failed", SasdStatusSeverity.Error, 10);
            }
        };
        commandBindings.Add(binding);
    }

    private async void OnShown(object? sender, EventArgs e)
    {
        themeService.Apply(this);
        try
        {
            await formStateService.RestoreAsync(this).ConfigureAwait(true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            ShowStatus("Saved window state could not be restored", SasdStatusSeverity.Warning, 5, TimeSpan.FromSeconds(5));
        }
    }

    private void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        try
        {
            formStateService.SaveAsync(this).GetAwaiter().GetResult();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // UI state is optional and must never block application shutdown.
        }
    }

    private void ApplySelectedTheme(int selectedIndex)
    {
        SasdThemeDefinition theme = selectedIndex switch
        {
            1 => SasdThemeCatalog.Dark,
            2 => SasdThemeCatalog.HighContrast,
            _ => SasdThemeCatalog.Light,
        };

        themeService.SetThemeAndApply(theme, this);
        ShowStatus($"Theme: {theme.Id}", Lifetime: TimeSpan.FromSeconds(3));
    }

    private void FilterCustomers(string searchText)
    {
        IEnumerable<CustomerRow> filtered = customers;
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = customers.Where(customer =>
                customer.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                customer.Contact.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                customer.Status.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
        }

        customerGrid.DataSource = filtered.ToList();
        ShowStatus($"Customers shown: {customerGrid.Rows.Count}", Lifetime: TimeSpan.FromSeconds(2));
    }

    private async Task ExportCustomersAsync()
    {
        var path = fileDialogService.SaveFile(
            this,
            new SasdFileDialogOptions("Export customers", "CSV files (*.csv)|*.csv", DefaultExtension: "csv"),
            "customers.csv");
        if (path is null)
        {
            return;
        }

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await SasdCsvExporter.ExportAsync(customerGrid, stream).ConfigureAwait(true);
        ShowStatus("CSV export completed", SasdStatusSeverity.Success, 2, TimeSpan.FromSeconds(4));
    }

    private async Task SaveGridLayoutAsync()
    {
        await stateStore.SaveAsync("gallery:grid:customers", SasdDataGridState.Capture(customerGrid)).ConfigureAwait(true);
        ShowStatus("Grid layout saved", SasdStatusSeverity.Success, 2, TimeSpan.FromSeconds(3));
    }

    private async Task RestoreGridLayoutAsync()
    {
        var state = await stateStore.LoadAsync<SasdDataGridState>("gallery:grid:customers").ConfigureAwait(true);
        if (state is null)
        {
            ShowStatus("No saved grid layout exists yet", SasdStatusSeverity.Information, 1, TimeSpan.FromSeconds(3));
            return;
        }

        state.Restore(customerGrid);
        ShowStatus("Grid layout restored", SasdStatusSeverity.Success, 2, TimeSpan.FromSeconds(3));
    }

    private void ShowStatus(
        string text,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        int priority = 0,
        TimeSpan? Lifetime = null) =>
        statusBar.ShowMessage(new SasdStatusMessage(text, severity, priority, Lifetime));

    private static IReadOnlyList<CustomerRow> CreateCustomers() =>
    [
        new(10001, "Muster GmbH", "Max Mustermann", "Active", 128450m),
        new(10002, "Beispiel AG", "Erika Beispiel", "Active", 98750m),
        new(10003, "Demo KG", "Dieter Demo", "Inactive", 45230m),
        new(10004, "Testfirma GmbH", "Tina Test", "Active", 77120m),
        new(10005, "SASD Solutions", "Marco Admin", "Active", 315900m),
    ];

    private sealed record CustomerRow(int Id, string Name, string Contact, string Status, decimal Revenue);
}
