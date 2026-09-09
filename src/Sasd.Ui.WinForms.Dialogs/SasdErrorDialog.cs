using Sasd.Ui.WinForms;

namespace Sasd.Ui.WinForms.Dialogs;

/// <summary>Displays a user-safe error with optional copyable technical details.</summary>
public sealed class SasdErrorDialog : SasdForm
{
    private readonly TextBox detailsTextBox;

    /// <summary>Initialises an error dialog.</summary>
    public SasdErrorDialog(string title, string message, string? technicalDetails = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        MinimumSize = new Size(560, 280);
        ClientSize = new Size(680, technicalDetails is null ? 250 : 430);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = technicalDetails is null ? 2 : 4,
            Padding = new Padding(20),
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var messageLabel = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            MaximumSize = new Size(620, 0),
            Text = message,
        };
        root.Controls.Add(messageLabel);

        detailsTextBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both,
            WordWrap = false,
            Text = technicalDetails ?? string.Empty,
            Visible = technicalDetails is not null,
        };

        if (technicalDetails is not null)
        {
            var detailsLabel = new Label
            {
                AutoSize = true,
                Text = "Technical details",
                Margin = new Padding(0, 16, 0, 6),
            };
            root.Controls.Add(detailsLabel);
            root.Controls.Add(detailsTextBox);
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        }

        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Margin = new Padding(0, 16, 0, 0),
        };

        var closeButton = new Button
        {
            AutoSize = true,
            DialogResult = DialogResult.OK,
            Text = "Close",
        };
        buttons.Controls.Add(closeButton);

        if (technicalDetails is not null)
        {
            var copyButton = new Button
            {
                AutoSize = true,
                Text = "Copy details",
            };
            copyButton.Click += (_, _) => Clipboard.SetText(detailsTextBox.Text);
            buttons.Controls.Add(copyButton);
        }

        root.Controls.Add(buttons);
        Controls.Add(root);
        AcceptButton = closeButton;
        CancelButton = closeButton;
    }
}
