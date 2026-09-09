using System.ComponentModel;
using System.Net.Mail;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.Sample.Crud;

/// <summary>
/// Small in-memory CRUD application used as a realistic consumer of the SASD UI Platform.
/// </summary>
/// <remarks>
/// <para>
/// The sample deliberately keeps persistence out of scope. Its purpose is to verify that a normal
/// application can compose grid/search, commands, dialogs, validation and status feedback without
/// needing application-specific copies of platform controls.
/// </para>
/// <para>
/// The code is intentionally explicit. A production application may introduce its own repository,
/// view-model or service layer, but those patterns are application concerns rather than requirements
/// imposed by the UI component library.
/// </para>
/// </remarks>
internal sealed class CrudSampleForm : SasdForm
{
    private readonly List<CustomerRecord> customers = CreateInitialCustomers();
    private readonly BindingSource bindingSource = new();
    private readonly SasdDialogService dialogService = new();
    private readonly SasdSearchBox searchBox = new();
    private readonly SasdDataGrid grid = new();
    private readonly SasdValidationSummary validationSummary = new();
    private readonly TextBox nameTextBox = new();
    private readonly TextBox emailTextBox = new();
    private readonly ComboBox statusComboBox = new();
    private readonly SasdStatusBar statusBar = new();
    private readonly SasdCommandBar commandBar = new();
    private readonly SasdCommand deleteCommand;
    private readonly SasdValidationCoordinator validation;

    private Guid? editingCustomerId;

    /// <summary>Initialises the reference application and its deterministic sample data.</summary>
    public CrudSampleForm()
    {
        Text = "SASD UI Platform — CRUD Reference";
        StateKey = "Sample.Crud.Main";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(880, 560);
        ClientSize = new Size(1080, 680);

        ConfigureGrid();
        ConfigureEditors();

        validation = new SasdValidationCoordinator(this);
        validation.AddRequired("name", nameTextBox, "Name");
        validation.AddRequired("email", emailTextBox, "Email");
        validation.AddRule("email-format", emailTextBox, _ =>
        {
            string value = emailTextBox.Text.Trim();
            if (value.Length == 0 || MailAddress.TryCreate(value, out _))
            {
                return ValueTask.FromResult<SasdValidationMessage?>(null);
            }

            return ValueTask.FromResult<SasdValidationMessage?>(
                new SasdValidationMessage("email-format", "Email must be a valid email address."));
        });

        var newCommand = new SasdCommand(
            "crud.new",
            "New",
            _ =>
            {
                BeginNewRecord();
                return Task.CompletedTask;
            },
            "Clear the editor and start a new record.");

        var saveCommand = new SasdCommand(
            "crud.save",
            "Save",
            SaveAsync,
            "Validate and save the edited record.");

        deleteCommand = new SasdCommand(
            "crud.delete",
            "Delete",
            _ =>
            {
                DeleteCurrentRecord();
                return Task.CompletedTask;
            },
            "Delete the selected record after confirmation.")
        {
            Enabled = false,
        };

        commandBar.AddCommand(newCommand);
        commandBar.AddCommand(saveCommand);
        commandBar.AddSeparator();
        commandBar.AddCommand(deleteCommand);
        commandBar.CommandCompleted += OnCommandCompleted;

        searchBox.Dock = DockStyle.Top;
        searchBox.PlaceholderText = "Search name, email or status...";
        searchBox.SearchTextChanged += OnSearchTextChanged;
        grid.SelectionChanged += OnGridSelectionChanged;

        Controls.Add(CreateMainLayout());
        Controls.Add(commandBar);
        Controls.Add(statusBar);
        commandBar.Dock = DockStyle.Top;
        statusBar.Dock = DockStyle.Bottom;

        ApplyFilter();
        BeginNewRecord();
        ShowStatus("CRUD reference application ready.", SasdStatusSeverity.Success);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            searchBox.SearchTextChanged -= OnSearchTextChanged;
            grid.SelectionChanged -= OnGridSelectionChanged;
            commandBar.CommandCompleted -= OnCommandCompleted;
            validation.Dispose();
            bindingSource.Dispose();
        }

        base.Dispose(disposing);
    }

    private Control CreateMainLayout()
    {
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel2,
            Orientation = Orientation.Vertical,
            Panel1MinSize = 420,
            Panel2MinSize = 320,
            SplitterDistance = 660,
        };
        split.Panel1.Padding = new Padding(12);
        split.Panel2.Padding = new Padding(12);

        var listLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
        };
        listLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        listLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        listLayout.Controls.Add(searchBox, 0, 0);
        listLayout.Controls.Add(grid, 0, 1);
        split.Panel1.Controls.Add(listLayout);

        var editorLayout = new TableLayoutPanel
        {
            AutoScroll = true,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
        };
        editorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        editorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        editorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        editorLayout.Controls.Add(CreateEditorHeading(), 0, 0);
        editorLayout.Controls.Add(validationSummary, 0, 1);
        editorLayout.Controls.Add(CreateFieldLayout(), 0, 2);
        split.Panel2.Controls.Add(editorLayout);

        return split;
    }

    private static Label CreateEditorHeading() => new()
    {
        AutoSize = true,
        Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold),
        Margin = new Padding(0, 0, 0, 12),
        Text = "Customer editor",
    };

    private Control CreateFieldLayout()
    {
        var fields = new SasdFieldLayout
        {
            Dock = DockStyle.Top,
        };
        fields.AddField("Name", nameTextBox, required: true);
        fields.AddField("Email", emailTextBox, required: true);
        fields.AddField("Status", statusComboBox);
        return fields;
    }

    private void ConfigureEditors()
    {
        nameTextBox.AccessibleName = "Customer name";
        emailTextBox.AccessibleName = "Customer email";
        statusComboBox.AccessibleName = "Customer status";
        statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusComboBox.Items.AddRange(["Active", "Paused", "Archived"]);
        statusComboBox.SelectedIndex = 0;
    }

    private void ConfigureGrid()
    {
        grid.AutoGenerateColumns = false;
        grid.Dock = DockStyle.Fill;
        grid.FillAvailableWidth = true;
        grid.ReadOnly = true;
        grid.DataSource = bindingSource;
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(CustomerRecord.Name),
            HeaderText = "Name",
            Name = "name",
            FillWeight = 35,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(CustomerRecord.Email),
            HeaderText = "Email",
            Name = "email",
            FillWeight = 45,
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(CustomerRecord.Status),
            HeaderText = "Status",
            Name = "status",
            FillWeight = 20,
        });
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        SasdValidationResult result = await validation.ValidateAsync(cancellationToken);
        validationSummary.ShowResult(result);
        if (!result.IsValid)
        {
            ShowStatus("Please correct the validation errors before saving.", SasdStatusSeverity.Error, priority: 10);
            return;
        }

        string name = nameTextBox.Text.Trim();
        string email = emailTextBox.Text.Trim();
        string status = statusComboBox.SelectedItem?.ToString() ?? "Active";

        CustomerRecord? record = editingCustomerId is { } id
            ? customers.FirstOrDefault(item => item.Id == id)
            : null;

        if (record is null)
        {
            record = new CustomerRecord
            {
                Name = name,
                Email = email,
                Status = status,
            };
            customers.Add(record);
            editingCustomerId = record.Id;
            ShowStatus("Customer created.", SasdStatusSeverity.Success);
        }
        else
        {
            record.Name = name;
            record.Email = email;
            record.Status = status;
            ShowStatus("Customer updated.", SasdStatusSeverity.Success);
        }

        ApplyFilter(record.Id);
        UpdateCommandState();
    }

    private void DeleteCurrentRecord()
    {
        if (editingCustomerId is not { } id)
        {
            return;
        }

        CustomerRecord? record = customers.FirstOrDefault(item => item.Id == id);
        if (record is null)
        {
            BeginNewRecord();
            return;
        }

        if (!dialogService.Confirm(this, $"Delete '{record.Name}'?", "Delete customer"))
        {
            ShowStatus("Delete cancelled.");
            return;
        }

        customers.Remove(record);
        ApplyFilter();
        BeginNewRecord();
        ShowStatus("Customer deleted.", SasdStatusSeverity.Success);
    }

    private void BeginNewRecord()
    {
        editingCustomerId = null;
        validation.ClearErrors();
        validationSummary.Clear();
        grid.ClearSelection();
        nameTextBox.Clear();
        emailTextBox.Clear();
        statusComboBox.SelectedIndex = 0;
        nameTextBox.Focus();
        UpdateCommandState();
    }

    private void LoadRecord(CustomerRecord record)
    {
        editingCustomerId = record.Id;
        validation.ClearErrors();
        validationSummary.Clear();
        nameTextBox.Text = record.Name;
        emailTextBox.Text = record.Email;
        statusComboBox.SelectedItem = record.Status;
        if (statusComboBox.SelectedIndex < 0)
        {
            statusComboBox.SelectedIndex = 0;
        }

        UpdateCommandState();
    }

    private void ApplyFilter(Guid? selectId = null)
    {
        string search = searchBox.SearchText.Trim();
        IEnumerable<CustomerRecord> result = customers;
        if (search.Length > 0)
        {
            result = result.Where(record =>
                Contains(record.Name, search) ||
                Contains(record.Email, search) ||
                Contains(record.Status, search));
        }

        bindingSource.DataSource = new BindingList<CustomerRecord>(result.ToList());
        bindingSource.ResetBindings(false);

        if (selectId is { } id)
        {
            SelectRow(id);
        }
    }

    private void SelectRow(Guid id)
    {
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.DataBoundItem is CustomerRecord record && record.Id == id)
            {
                row.Selected = true;
                grid.CurrentCell = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(cell => cell.Visible);
                return;
            }
        }
    }

    private void UpdateCommandState() => deleteCommand.Enabled = editingCustomerId is not null;

    private void OnSearchTextChanged(object? sender, EventArgs e) => ApplyFilter(editingCustomerId);

    private void OnGridSelectionChanged(object? sender, EventArgs e)
    {
        if (grid.CurrentRow?.DataBoundItem is CustomerRecord record)
        {
            LoadRecord(record);
        }
    }

    private void OnCommandCompleted(object? sender, SasdCommandBarCommandCompletedEventArgs e)
    {
        if (!e.Result.Succeeded)
        {
            ShowStatus(e.Result.UserMessage ?? "The command could not be completed.", SasdStatusSeverity.Error, priority: 10);
        }
    }

    private void ShowStatus(
        string text,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        int priority = 0) =>
        statusBar.ShowMessage(new SasdStatusMessage(text, severity, priority, TimeSpan.FromSeconds(4)));

    private static bool Contains(string value, string search) =>
        value.Contains(search, StringComparison.CurrentCultureIgnoreCase);

    private static List<CustomerRecord> CreateInitialCustomers() =>
    [
        new CustomerRecord { Name = "Example AG", Email = "contact@example.test", Status = "Active" },
        new CustomerRecord { Name = "Northwind Demo", Email = "hello@northwind.test", Status = "Paused" },
        new CustomerRecord { Name = "Contoso Sample", Email = "info@contoso.test", Status = "Active" },
        new CustomerRecord { Name = "Archived Customer", Email = "archive@example.test", Status = "Archived" },
    ];
}
