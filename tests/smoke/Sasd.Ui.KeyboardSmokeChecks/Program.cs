using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Data;

namespace Sasd.Ui.KeyboardSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateTabTraversalAndShortcut();
            SearchBoxRequestChecks.Run();
            Console.WriteLine("SASD keyboard acceptance smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateTabTraversalAndShortcut()
    {
        using var form = new KeyboardScenarioForm();
        var manager = new SasdCommandManager();
        int shortcutExecutions = 0;
        int shortcutCompletions = 0;

        var command = new SasdCommand(
            "keyboard.refresh",
            "Refresh",
            _ =>
            {
                shortcutExecutions++;
                return Task.CompletedTask;
            },
            shortcutKeys: Keys.Control | Keys.Shift | Keys.R);
        manager.Register(command);

        using var shortcutBinding = new SasdCommandShortcutBinding(form, manager);
        shortcutBinding.CommandCompleted += (_, args) =>
        {
            Ensure(args.Result.Succeeded, "Keyboard shortcut command completed with an error result.");
            Ensure(ReferenceEquals(args.Command, command), "Keyboard shortcut completion reported the wrong command.");
            shortcutCompletions++;
        };

        // The form is shown off-screen so normal WinForms dialog-key handling has a
        // native form/handle context without flashing a test window on developer or CI
        // desktops. We deliberately do not use SendKeys: SendKeys injects input into a
        // process-global desktop session and is much more sensitive to runner focus.
        form.Show();

        form.ActiveControl = form.NameTextBox;
        Ensure(form.RouteDialogKey(Keys.Tab), "WinForms did not accept the first Tab dialog key.");
        Ensure(ReferenceEquals(form.ActiveControl, form.EnabledCheckBox),
            "Tab did not move from the name field to the enabled option.");

        Ensure(form.RouteDialogKey(Keys.Tab), "WinForms did not accept the second Tab dialog key.");
        Ensure(ReferenceEquals(form.ActiveControl, form.ApplyButton),
            "Tab did not move from the enabled option to the apply button.");

        Ensure(form.RouteDialogKey(Keys.Shift | Keys.Tab), "WinForms did not accept Shift+Tab.");
        Ensure(ReferenceEquals(form.ActiveControl, form.EnabledCheckBox),
            "Shift+Tab did not move focus backward to the enabled option.");

        // The platform shortcut binding listens on the normal Form.KeyDown path and
        // enables KeyPreview so a global command can be invoked while a child control
        // owns the current input focus. Raising the protected WinForms event in this
        // test subclass avoids a private product hook while keeping the scenario local
        // and deterministic.
        KeyEventArgs shortcutEvent = form.RouteKeyDown(Keys.Control | Keys.Shift | Keys.R);
        Ensure(shortcutEvent.Handled && shortcutEvent.SuppressKeyPress,
            "The registered global shortcut was not claimed and suppressed.");
        Ensure(shortcutExecutions == 1, "The registered global shortcut did not execute exactly once.");
        Ensure(shortcutCompletions == 1, "The shortcut binding did not publish exactly one completion event.");

        command.Enabled = false;
        KeyEventArgs disabledShortcut = form.RouteKeyDown(Keys.Control | Keys.Shift | Keys.R);
        Ensure(!disabledShortcut.Handled && !disabledShortcut.SuppressKeyPress,
            "A disabled command incorrectly claimed its global shortcut.");
        Ensure(shortcutExecutions == 1, "A disabled keyboard command was executed.");

        int searchChanges = 0;
        form.SearchBox.SearchTextChanged += (_, _) => searchChanges++;
        form.SearchBox.SearchText = "release";
        Ensure(form.SearchBox.SearchText == "release" && searchChanges == 1,
            "The search box did not retain and publish its search text change.");

        ValidateSearchBoxKeyboardAndAccessibility(form, () => searchChanges);

        form.Hide();
        form.Dispose();
        Ensure(form.IsDisposed, "Keyboard scenario form was not disposed.");
        Ensure(form.NameTextBox.IsDisposed && form.EnabledCheckBox.IsDisposed && form.ApplyButton.IsDisposed,
            "The form did not dispose its owned keyboard-route controls.");
    }

    private static void ValidateSearchBoxKeyboardAndAccessibility(
        KeyboardScenarioForm form,
        Func<int> getSearchChangeCount)
    {
        KeyboardSearchBox searchBox = form.SearchBox;
        TextBox editor = searchBox.Controls.OfType<TextBox>().Single();
        Button clearButton = searchBox.Controls.OfType<Button>().Single();
        int searchRequests = 0;
        string? lastRequestedText = null;
        searchBox.SearchRequested += (_, args) =>
        {
            searchRequests++;
            lastRequestedText = args.SearchText;
        };

        Ensure(searchBox.AccessibleRole == AccessibleRole.Grouping && searchBox.AccessibleName == "Search",
            "Search box does not expose stable group-level accessibility semantics.");
        Ensure(editor.AccessibleName == "Search text",
            "Search editor does not expose an explicit accessible name.");
        Ensure(clearButton.AccessibleName == "Clear search" &&
               !string.IsNullOrWhiteSpace(clearButton.AccessibleDescription),
            "Search clear action does not expose a descriptive accessible contract.");
        Ensure(clearButton.Visible && clearButton.CanSelect,
            "Search clear action is not keyboard-selectable while search text is present.");

        // FocusSearch is part of the public component contract. In a real key path the native
        // editor delegates its dialog key to the immediate container (SasdSearchBox), not to
        // the outer Form. The test subclass only exposes that protected WinForms path; it does
        // not add product behavior or a public testing hook.
        searchBox.FocusSearch();
        Ensure(ReferenceEquals(searchBox.ActiveControl, editor),
            "FocusSearch did not move focus to the native search editor.");
        Ensure(searchBox.RouteDialogKey(Keys.Tab),
            "WinForms did not accept Tab from the search editor to its clear action.");
        Ensure(ReferenceEquals(searchBox.ActiveControl, clearButton),
            "Tab from the search editor did not reach the visible clear action.");

        Ensure(searchBox.RouteDialogKey(Keys.Shift | Keys.Tab),
            "WinForms did not accept Shift+Tab from the search clear action.");
        Ensure(ReferenceEquals(searchBox.ActiveControl, editor),
            "Shift+Tab from the search clear action did not return to the editor.");

        // Enter is a deliberate local commit shortcut while the search editor owns focus. It
        // flushes the pending debounce rather than allowing an enclosing form's default button
        // to accidentally become the search action.
        Ensure(searchBox.RouteCommandKey(Keys.Enter),
            "Search editor did not claim Enter for an immediate search request.");
        Ensure(searchRequests == 1 && lastRequestedText == "release",
            "Enter did not commit the current search text exactly once.");

        // PerformClick exercises the same public clear action after proving that keyboard
        // traversal can reach it. Clearing raises the immediate text-change event and one
        // immediate empty-state search request, then removes the inapplicable action from the
        // keyboard/accessibility surface.
        clearButton.PerformClick();
        Ensure(searchBox.SearchText.Length == 0,
            "Search clear action did not clear the current search text.");
        Ensure(getSearchChangeCount() == 2,
            "Search clear action did not publish exactly one additional text-change notification.");
        Ensure(searchRequests == 2 && lastRequestedText == string.Empty,
            "Search clear action did not request the empty search state exactly once.");
        Ensure(!clearButton.Visible && !clearButton.CanSelect,
            "Empty search box left an inapplicable clear action in keyboard navigation.");

        searchBox.SearchText = "escape";
        searchBox.FocusSearch();
        Ensure(searchBox.RouteCommandKey(Keys.Escape),
            "Non-empty search did not claim Escape for clearing.");
        Ensure(searchBox.SearchText.Length == 0 && getSearchChangeCount() == 4,
            "Escape did not clear the search through the normal text-change contract.");
        Ensure(searchRequests == 3 && lastRequestedText == string.Empty,
            "Escape did not replace the pending debounce with one empty-state request.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    /// <summary>
    /// Small application-like keyboard surface used to exercise WinForms' own dialog-key
    /// traversal together with the SASD global-shortcut and search components.
    /// </summary>
    private sealed class KeyboardScenarioForm : Form
    {
        public KeyboardScenarioForm()
        {
            Text = "SASD keyboard acceptance smoke";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(-32000, -32000);
            ClientSize = new Size(520, 220);

            NameTextBox = new TextBox
            {
                AccessibleName = "Name",
                Dock = DockStyle.Top,
                TabIndex = 0,
            };

            EnabledCheckBox = new CheckBox
            {
                AccessibleName = "Enabled",
                AutoSize = true,
                Checked = true,
                TabIndex = 1,
                Text = "Enabled",
            };

            ApplyButton = new Button
            {
                AccessibleName = "Apply",
                AutoSize = true,
                TabIndex = 2,
                Text = "Apply",
            };

            SearchBox = new KeyboardSearchBox
            {
                Dock = DockStyle.Top,
                TabIndex = 3,
            };

            var layout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                RowCount = 4,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(NameTextBox, 0, 0);
            layout.Controls.Add(EnabledCheckBox, 0, 1);
            layout.Controls.Add(ApplyButton, 0, 2);
            layout.Controls.Add(SearchBox, 0, 3);
            Controls.Add(layout);
        }

        public TextBox NameTextBox { get; }

        public CheckBox EnabledCheckBox { get; }

        public Button ApplyButton { get; }

        public KeyboardSearchBox SearchBox { get; }

        public bool RouteDialogKey(Keys keyData) => ProcessDialogKey(keyData);

        public KeyEventArgs RouteKeyDown(Keys keyData)
        {
            var args = new KeyEventArgs(keyData);
            OnKeyDown(args);
            return args;
        }
    }

    /// <summary>
    /// Test-only adapter exposing inherited protected key routes. Keeping the adapter in test
    /// code avoids adding a public automation hook while exercising the same immediate
    /// container paths that native WinForms children use for Tab/Enter/Escape processing.
    /// </summary>
    private sealed class KeyboardSearchBox : SasdSearchBox
    {
        public bool RouteDialogKey(Keys keyData) => ProcessDialogKey(keyData);

        public bool RouteCommandKey(Keys keyData)
        {
            Message message = default;
            return ProcessCmdKey(ref message, keyData);
        }
    }
}
