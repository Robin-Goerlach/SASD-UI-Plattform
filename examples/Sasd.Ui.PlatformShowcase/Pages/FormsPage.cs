using System.Net.Mail;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Demonstrates field layout and validation as one normal form workflow.</summary>
internal sealed class FormsPage : UserControl
{
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly TextBox nameTextBox = new();
    private readonly TextBox emailTextBox = new();
    private readonly ComboBox roleComboBox = new();
    private readonly SasdValidationSummary validationSummary = new();
    private readonly SasdValidationCoordinator validation;

    public FormsPage(Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        nameTextBox.AccessibleName = "Display name";
        emailTextBox.AccessibleName = "Email address";
        roleComboBox.AccessibleName = "Role";
        roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        roleComboBox.Items.AddRange(["Administrator", "Developer", "Operator", "Reader"]);
        roleComboBox.SelectedIndex = 1;

        validation = new SasdValidationCoordinator(this);
        validation.AddRequired("name", nameTextBox, "Name");
        validation.AddRequired("email", emailTextBox, "Email");
        validation.AddRule("email-format", emailTextBox, cancellationToken =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            string value = emailTextBox.Text.Trim();
            if (value.Length == 0 || MailAddress.TryCreate(value, out _))
            {
                return ValueTask.FromResult<SasdValidationMessage?>(null);
            }

            return ValueTask.FromResult<SasdValidationMessage?>(
                new SasdValidationMessage("email-format", "Email must be a valid email address."));
        });

        // Binding keeps result display and summary-to-field navigation on the shared validation
        // contracts. The page still owns/disposes the coordinator; the summary owns only its
        // event subscription and detaches it explicitly during page disposal.
        validationSummary.Bind(validation);

        var fields = new SasdFieldLayout
        {
            Dock = DockStyle.Top,
        };
        fields.AddField("Name", nameTextBox, required: true);
        fields.AddField("Email", emailTextBox, required: true);
        fields.AddField("Role", roleComboBox);

        var validateButton = new Button
        {
            AutoSize = true,
            Text = "Validate",
        };
        validateButton.Click += OnValidateClick;

        var resetButton = new Button
        {
            AutoSize = true,
            Text = "Reset",
        };
        resetButton.Click += (_, _) => ResetForm();

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 12, 0, 0),
        };
        actions.Controls.Add(validateButton);
        actions.Controls.Add(resetButton);

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(850, 0),
            Text =
                "Forms and validation\r\n\r\n" +
                "Required fields, custom rules and the validation summary use shared platform contracts. " +
                "Try an empty form, an invalid email address and then a valid form. Activate a summary message with Enter or double-click to return to its field.",
        };

        var layout = new TableLayoutPanel
        {
            AutoScroll = true,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 4,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(validationSummary, 0, 1);
        layout.Controls.Add(fields, 0, 2);
        layout.Controls.Add(actions, 0, 3);
        Controls.Add(layout);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            validationSummary.Unbind();
            validation.Dispose();
        }

        base.Dispose(disposing);
    }

    private async void OnValidateClick(object? sender, EventArgs e)
    {
        SasdValidationResult result = await validation.ValidateAsync(CancellationToken.None);

        publishStatus(
            result.IsValid ? "Form validation passed." : $"Form validation found {result.Messages.Count} issue(s).",
            result.IsValid ? SasdStatusSeverity.Success : SasdStatusSeverity.Warning,
            TimeSpan.FromSeconds(4));
    }

    private void ResetForm()
    {
        nameTextBox.Clear();
        emailTextBox.Clear();
        roleComboBox.SelectedIndex = 1;
        validationSummary.Clear();
        nameTextBox.Focus();
        publishStatus("Form reset.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
    }
}
