using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Theming;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Provides a dependency-free manual exercise surface for DPI, keyboard focus and
/// high-contrast observations that are difficult to prove from ordinary unit-style checks.
/// </summary>
/// <remarks>
/// This page deliberately reports what the running process can observe, but it does not
/// mark a release gate as passed. Formal acceptance evidence still has to record the machine,
/// display scale, Visual Studio/Windows configuration and the human observation separately.
/// </remarks>
internal sealed class AcceptanceLabPage : UserControl
{
    private readonly SasdThemeService themeService;
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly TextBox runtimeSnapshot = new();
    private readonly ListBox focusLog = new();
    private readonly List<Control> focusControls = [];

    public AcceptanceLabPage(
        SasdThemeService themeService,
        Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        themeService.ThemeChanged += OnThemeChanged;
        SizeChanged += OnSizeChanged;
        DpiChangedAfterParent += OnDpiChangedAfterParent;

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(980, 0),
            Text =
                "Manual acceptance lab\r\n\r\n" +
                "Use this page while checking different Windows display scales, keyboard-only operation and High Contrast. " +
                "The values below are a live observation aid, not stored release evidence. A green CI build also does not replace the manual checks.",
        };

        var refreshButton = new Button
        {
            AutoSize = true,
            Text = "Refresh runtime snapshot",
        };
        refreshButton.Click += (_, _) =>
        {
            RefreshRuntimeSnapshot();
            publishStatus("Acceptance snapshot refreshed.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
        };

        var clearFocusButton = new Button
        {
            AutoSize = true,
            Text = "Clear focus log",
        };
        clearFocusButton.Click += (_, _) => focusLog.Items.Clear();

        var actions = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 8, 0, 8),
        };
        actions.Controls.Add(refreshButton);
        actions.Controls.Add(clearFocusButton);

        var runtimeSection = new SasdSectionPanel
        {
            Dock = DockStyle.Fill,
            SectionTitle = "Runtime DPI / theme snapshot",
        };
        runtimeSection.Controls.Add(CreateRuntimeArea());

        var keyboardSection = new SasdSectionPanel
        {
            Dock = DockStyle.Fill,
            SectionTitle = "Keyboard focus route",
        };
        keyboardSection.Controls.Add(CreateKeyboardArea());

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 520,
        };
        split.Panel1.Controls.Add(runtimeSection);
        split.Panel2.Controls.Add(keyboardSection);

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
        layout.Controls.Add(actions, 0, 1);
        layout.Controls.Add(split, 0, 2);
        Controls.Add(layout);

        RefreshRuntimeSnapshot();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            themeService.ThemeChanged -= OnThemeChanged;
            SizeChanged -= OnSizeChanged;
            DpiChangedAfterParent -= OnDpiChangedAfterParent;

            // Child controls normally disappear with this page, but explicitly removing the
            // callbacks documents the ownership boundary and keeps this sample useful as a
            // lifecycle example for application code.
            foreach (Control control in focusControls)
            {
                control.Enter -= OnFocusEntered;
            }

            focusControls.Clear();
        }

        base.Dispose(disposing);
    }

    private Control CreateRuntimeArea()
    {
        runtimeSnapshot.AccessibleName = "Runtime acceptance snapshot";
        runtimeSnapshot.Dock = DockStyle.Fill;
        runtimeSnapshot.Multiline = true;
        runtimeSnapshot.ReadOnly = true;
        runtimeSnapshot.ScrollBars = ScrollBars.Vertical;
        runtimeSnapshot.WordWrap = false;

        var checklist = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(470, 0),
            Text =
                "Suggested observations:\r\n" +
                "• repeat at 100%, 125%, 150% and 200% display scale;\r\n" +
                "• resize the window and look for clipping or overlapping controls;\r\n" +
                "• move between differently scaled monitors when available;\r\n" +
                "• compare SASD High Contrast with the real Windows High Contrast setting;\r\n" +
                "• record the result outside this page as formal evidence.",
        };

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
            Padding = new Padding(8),
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(runtimeSnapshot, 0, 0);
        layout.Controls.Add(checklist, 0, 1);
        return layout;
    }

    private Control CreateKeyboardArea()
    {
        var explanation = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(470, 0),
            Text =
                "Start in the first field and use Tab / Shift+Tab only. The focus log should follow the numbered order in both directions. " +
                "Also verify that Space activates the check box/button and that focus remains visible in each active theme.",
        };

        var nameTextBox = new TextBox
        {
            AccessibleName = "Step 1 name field",
            TabIndex = 0,
        };
        var roleComboBox = new ComboBox
        {
            AccessibleName = "Step 2 role selector",
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 1,
        };
        roleComboBox.Items.AddRange(["Developer", "Operator", "Reader"]);
        roleComboBox.SelectedIndex = 0;

        var enabledCheckBox = new CheckBox
        {
            AccessibleName = "Step 3 enabled option",
            AutoSize = true,
            Checked = true,
            TabIndex = 2,
            Text = "Enabled option",
        };
        var actionButton = new Button
        {
            AccessibleName = "Step 4 example action",
            AutoSize = true,
            TabIndex = 3,
            Text = "Example action",
        };
        actionButton.Click += (_, _) =>
            publishStatus("Keyboard-lab example action invoked.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(3));

        var notesTextBox = new TextBox
        {
            AccessibleName = "Step 5 notes field",
            TabIndex = 4,
        };

        RegisterFocusControl(nameTextBox);
        RegisterFocusControl(roleComboBox);
        RegisterFocusControl(enabledCheckBox);
        RegisterFocusControl(actionButton);
        RegisterFocusControl(notesTextBox);

        var fields = new SasdFieldLayout
        {
            Dock = DockStyle.Top,
        };
        fields.AddField("1. Name", nameTextBox);
        fields.AddField("2. Role", roleComboBox);
        fields.AddField("3. Option", enabledCheckBox);
        fields.AddField("4. Action", actionButton);
        fields.AddField("5. Notes", notesTextBox);

        focusLog.AccessibleName = "Keyboard focus log";
        focusLog.Dock = DockStyle.Fill;
        focusLog.HorizontalScrollbar = true;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(8),
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(explanation, 0, 0);
        layout.Controls.Add(fields, 0, 1);
        layout.Controls.Add(focusLog, 0, 2);
        return layout;
    }

    private void RegisterFocusControl(Control control)
    {
        focusControls.Add(control);
        control.Enter += OnFocusEntered;
    }

    private void OnFocusEntered(object? sender, EventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }

        string name = string.IsNullOrWhiteSpace(control.AccessibleName)
            ? control.GetType().Name
            : control.AccessibleName;
        focusLog.Items.Insert(0, $"{DateTime.Now:T}  {name}");
    }

    private void OnThemeChanged(object? sender, SasdThemeChangedEventArgs e) => RefreshRuntimeSnapshot();

    private void OnSizeChanged(object? sender, EventArgs e) => RefreshRuntimeSnapshot();

    private void OnDpiChangedAfterParent(object? sender, EventArgs e) => RefreshRuntimeSnapshot();

    private void RefreshRuntimeSnapshot()
    {
        if (IsDisposed || Disposing)
        {
            return;
        }

        Form? host = FindForm();
        Size hostClientSize = host?.ClientSize ?? Size.Empty;
        double scale = DeviceDpi / 96d;

        runtimeSnapshot.Lines =
        [
            $"Control DeviceDpi: {DeviceDpi}",
            $"Approximate scale factor: {scale:F2}x ({scale * 100:F0}%)",
            $"AutoScaleMode: {AutoScaleMode}",
            $"CurrentAutoScaleDimensions: {CurrentAutoScaleDimensions.Width:F1} x {CurrentAutoScaleDimensions.Height:F1}",
            $"Showcase client size: {hostClientSize.Width} x {hostClientSize.Height}",
            $"Current font: {Font.Name}, {Font.SizeInPoints:F1} pt",
            $"SASD theme id: {themeService.Current.Id}",
            $"SASD theme mode: {themeService.Current.Mode}",
            $"Windows High Contrast: {SystemInformation.HighContrast}",
            $"Menu access keys underlined: {SystemInformation.MenuAccessKeysUnderlined}",
            $"RightToLeft: {RightToLeft}",
        ];
    }
}
