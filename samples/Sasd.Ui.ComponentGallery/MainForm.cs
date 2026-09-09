using Sasd.Ui.Core;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Theming;

namespace Sasd.Ui.ComponentGallery;

/// <summary>
/// Executable R0 specification for the first reusable native WinForms
/// components. Each page is intentionally small enough to remain a practical
/// regression and adoption example.
/// </summary>
internal sealed class MainForm : SasdForm
{
    private readonly SasdThemeService themeService = new();
    private readonly ISasdDialogService dialogService = new SasdDialogService();
    private readonly Label statusLabel = new();
    private readonly SasdDataGrid customerGrid = new();
    private readonly IReadOnlyList<CustomerRow> customers = CreateCustomers();

    public MainForm()
    {
        StateKey = "ComponentGallery.MainWindow";
        Text = "SASD UI Platform — Component Gallery";
        MinimumSize = new Size(960, 640);
        ClientSize = new Size(1240, 780);
        StartPosition = FormStartPosition.CenterScreen;

        Controls.Add(CreateRootLayout());
        Shown += (_, _) => themeService.Apply(this);
    }

    private Control CreateRootLayout()
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

        statusLabel.AutoSize = true;
        statusLabel.Margin = new Padding(0, 8, 0, 0);
        statusLabel.Text = "Ready — native WinForms R0 foundation";
        root.Controls.Add(statusLabel, 0, 2);

        return root;
    }

    private Control CreateHeader()
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
        titlePanel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 20, FontStyle.Bold),
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

    private Control CreateTabs()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(CreateOverviewPage());
        tabs.TabPages.Add(CreateFormsPage());
        tabs.TabPages.Add(CreateDataPage());
        tabs.TabPages.Add(CreateFeedbackPage());
        return tabs;
    }

    private TabPage CreateOverviewPage()
    {
        var page = new TabPage("Overview") { Padding = new Padding(16) };
        var section = new SasdSectionPanel
        {
            Dock = DockStyle.Top,
            Height = 190,
            SectionTitle = "First usable foundation",
        };
        section.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            Text = "This R0 slice demonstrates real SASD components rather than placeholders:\r\n\r\n" +
                   "• built-in design tokens and native Light/Dark/High Contrast mapping\r\n" +
                   "• field layout and section components\r\n" +
                   "• search, empty-state and business-grid defaults\r\n" +
                   "• a replaceable dialog service\r\n\r\n" +
                   "Krypton remains isolated for the next visual comparison step.",
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
            Height = 250,
            SectionTitle = "SasdFieldLayout",
        };

        var fields = new SasdFieldLayout { Dock = DockStyle.Top, Padding = new Padding(12) };
        fields.AddField("Name", new TextBox { Text = "Example AG" }, required: true);
        fields.AddField("E-mail", new TextBox { Text = "contact@example.test" }, required: true);
        fields.AddField("Category", new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = new[] { "Customer", "Partner", "Internal" },
        });

        var saveButton = new Button { AutoSize = true, Text = "Validate sample" };
        saveButton.Click += (_, _) => dialogService.ShowInformation(
            this,
            "The native form components are wired and reusable.",
            "SASD UI Platform");
        int actionRow = fields.RowCount;
        fields.RowCount++;
        fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fields.Controls.Add(saveButton, 1, actionRow);

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
            RowCount = 2,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var search = new SasdSearchBox
        {
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 10),
            PlaceholderText = "Search customers...",
        };
        search.SearchTextChanged += (_, _) => FilterCustomers(search.SearchText);

        customerGrid.Dock = DockStyle.Fill;
        customerGrid.ReadOnly = true;
        customerGrid.DataSource = customers.ToList();

        layout.Controls.Add(search, 0, 0);
        layout.Controls.Add(customerGrid, 0, 1);
        page.Controls.Add(layout);
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
        buttons.Controls.Add(CreateDialogButton("Error", () =>
            dialogService.ShowError(this, "User-safe error presentation.", "Error")));
        buttons.Controls.Add(CreateDialogButton("Confirmation", () =>
        {
            bool confirmed = dialogService.Confirm(this, "Continue with the example action?", "Confirm");
            statusLabel.Text = confirmed ? "Confirmation accepted" : "Confirmation cancelled";
        }));

        var empty = new SasdEmptyState
        {
            Dock = DockStyle.Fill,
            Title = "No notifications",
            Message = "This is the standard empty state for a feature that currently contains no items.",
            ActionText = "Create example",
        };
        empty.ActionInvoked += (_, _) => statusLabel.Text = "Empty-state action invoked";

        layout.Controls.Add(buttons, 0, 0);
        layout.Controls.Add(empty, 1, 0);
        page.Controls.Add(layout);
        return page;
    }

    private static Button CreateDialogButton(string text, Action action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 8),
            MinimumSize = new Size(160, 32),
            Text = text,
        };
        button.Click += (_, _) => action();
        return button;
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
        statusLabel.Text = $"Theme: {theme.Id}";
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
        statusLabel.Text = $"Customers shown: {customerGrid.Rows.Count}";
    }

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
