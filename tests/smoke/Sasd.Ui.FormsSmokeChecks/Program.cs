using System.Reflection;
using Sasd.Ui.WinForms.Forms;

namespace Sasd.Ui.FormsSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateFieldLayout();
            ValidateSectionPanel();
            ValidateValidationFocusNavigation();

            Console.WriteLine("SASD forms smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateFieldLayout()
    {
        var layout = new SasdFieldLayout();
        var nameEditor = new TextBox();
        var customEditor = new TextBox
        {
            AccessibleName = "Application supplied editor name",
            AccessibleDescription = "Application supplied editor description",
            MinimumSize = new Size(20, 40),
        };

        try
        {
            Ensure(layout.AccessibleRole == AccessibleRole.Grouping,
                "Field layout does not expose itself as an accessible grouping.");
            Ensure(layout.FieldGap == 8, "Field layout default gap changed unexpectedly.");
            Ensure(layout.RowCount == 0, "New field layout unexpectedly contains rows.");

            Label nameLabel = layout.AddField("  Name  ", nameEditor, required: true);
            Ensure(layout.RowCount == 1 && layout.RowStyles.Count == 1,
                "Adding a field did not create exactly one layout row.");
            Ensure(nameLabel.Text == "Name *", "Required field did not receive the visible required marker.");
            Ensure(nameLabel.AccessibleName == "Name" && nameLabel.AccessibleDescription == "Required field.",
                "Required field label does not expose stable accessible text.");
            Ensure(nameEditor.AccessibleName == "Name" && nameEditor.AccessibleDescription == "Required field.",
                "Editor without application accessibility text did not receive conservative field defaults.");
            Ensure(nameEditor.Anchor == (AnchorStyles.Left | AnchorStyles.Right),
                "Field editor did not receive the expected horizontal anchoring.");
            Ensure(nameEditor.Margin == new Padding(0, 4, 0, 4),
                "Field editor spacing changed unexpectedly.");
            Ensure(nameEditor.MinimumSize.Height == 28,
                "Field editor without a minimum height did not receive the platform minimum.");

            Label customLabel = layout.AddField("Description", customEditor);
            Ensure(customEditor.AccessibleName == "Application supplied editor name",
                "Field layout overwrote an application supplied accessible name.");
            Ensure(customEditor.AccessibleDescription == "Application supplied editor description",
                "Field layout overwrote an application supplied accessible description.");
            Ensure(customEditor.MinimumSize.Height == 40,
                "Field layout replaced an application supplied minimum editor height.");
            Ensure(customLabel.AccessibleDescription is null,
                "Optional field was incorrectly announced as required.");

            layout.FieldGap = 18;
            Ensure(nameLabel.Margin.Right == 18 && customLabel.Margin.Right == 18,
                "Changing FieldGap did not update labels previously created by AddField.");

            EnsureThrows<ArgumentOutOfRangeException>(
                () => layout.FieldGap = -1,
                "Field layout accepted a negative field gap.");
            Ensure(layout.FieldGap == 18,
                "Rejected field-gap input changed the previously valid setting.");

            EnsureThrows<ArgumentException>(
                () => layout.AddField("   ", new TextBox()),
                "Field layout accepted an empty label.");
        }
        finally
        {
            // Editors become normal WinForms children when added to the layout. Disposing
            // the parent must therefore release them as well; this makes the ownership
            // transfer explicit and prevents ambiguous disposal expectations in consumers.
            layout.Dispose();
        }

        Ensure(nameEditor.IsDisposed && customEditor.IsDisposed,
            "Disposing the field layout did not dispose child editors it owned as parent.");
    }

    private static void ValidateSectionPanel()
    {
        using var section = new SasdSectionPanel();

        Ensure(section.AccessibleRole == AccessibleRole.Grouping,
            "Section panel does not expose an accessible grouping role.");
        Ensure(!section.TabStop, "Section panel unexpectedly participates in tab traversal.");
        Ensure(section.Padding == new Padding(12, 20, 12, 12),
            "Section panel padding changed unexpectedly.");
        Ensure(section.Margin == new Padding(0, 0, 0, 12),
            "Section panel margin changed unexpectedly.");

        section.SectionTitle = "Customer";
        Ensure(section.Text == "Customer", "SectionTitle did not project to the native GroupBox title.");

        section.SectionTitle = null!;
        Ensure(section.Text == string.Empty, "Null section title was not normalized to an empty title.");
    }

    private static void ValidateValidationFocusNavigation()
    {
        using var form = new Form
        {
            ClientSize = new Size(640, 360),
            Location = new Point(-32000, -32000),
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Text = "SASD validation navigation smoke",
        };
        using var summary = new SasdValidationSummary { Dock = DockStyle.Top };
        using var fields = new Panel { AutoScroll = true, Dock = DockStyle.Fill };
        var nameEditor = new TextBox
        {
            AccessibleName = "Name",
            Location = new Point(12, 220),
            Width = 280,
        };
        var emailEditor = new TextBox
        {
            AccessibleName = "Email",
            Location = new Point(12, 12),
            Width = 280,
        };
        fields.Controls.Add(nameEditor);
        fields.Controls.Add(emailEditor);
        form.Controls.Add(fields);
        form.Controls.Add(summary);

        using var coordinator = new SasdValidationCoordinator(form);
        coordinator.AddRequired("name", nameEditor, "Name");
        coordinator.AddRequired("email", emailEditor, "Email");
        summary.Bind(coordinator);

        form.Show();
        emailEditor.Focus();
        Ensure(emailEditor.Focused,
            "Validation-navigation smoke could not establish its initial focus target.");

        // Both rules complete synchronously, so this call does not require a separate message
        // loop. Binding proves that the summary receives ValidationCompleted without application
        // code having to duplicate ShowResult plumbing.
        SasdValidationResult result = coordinator.ValidateAsync().GetAwaiter().GetResult();
        Ensure(result.Messages.Count == 2 && summary.Visible,
            "Bound validation summary did not display the completed validation result.");

        ListBox messageList = FindDescendant<ListBox>(summary)
            ?? throw new InvalidOperationException("Validation summary did not create its message list.");
        Ensure(messageList.Items.Count == 2 && messageList.Items[0] is SasdValidationMessage,
            "Validation summary did not retain full validation-message objects for navigation.");
        Ensure(summary.AccessibleDescription?.Contains("Error: Name is required.", StringComparison.Ordinal) == true,
            "Validation summary accessible text omitted message severity.");

        SasdValidationMessageInvokedEventArgs? invoked = null;
        summary.MessageInvoked += (_, args) => invoked = args;
        messageList.SelectedIndex = 0;
        messageList.Focus();
        Ensure(messageList.Focused,
            "Validation message list could not receive keyboard focus in a real WinForms host.");

        KeyEventArgs enter = RaiseKeyDown(messageList, Keys.Enter);
        Ensure(enter.Handled && enter.SuppressKeyPress,
            "Enter activation was not claimed by the validation message list.");
        Ensure(nameEditor.Focused,
            "Activating the name validation message did not move focus to its registered editor.");
        Ensure(invoked?.Message.FieldKey == "name" && invoked.FocusMoved,
            "Validation message activation did not report the selected message and successful focus navigation.");

        Ensure(coordinator.TryFocusField("EMAIL") && emailEditor.Focused,
            "Validation field lookup was not case-insensitive or did not focus the matching editor.");
        Ensure(!coordinator.TryFocusField("missing"),
            "Validation coordinator reported success for an unknown field key.");

        emailEditor.Enabled = false;
        Ensure(!coordinator.TryFocusField("email"),
            "Validation coordinator reported success for a disabled/unselectable target.");
        emailEditor.Enabled = true;

        summary.Unbind();
        summary.Clear();
        _ = coordinator.ValidateAsync().GetAwaiter().GetResult();
        Ensure(!summary.Visible && messageList.Items.Count == 0,
            "Validation summary continued receiving results after Unbind.");

        form.Hide();
    }

    private static KeyEventArgs RaiseKeyDown(Control control, Keys keys)
    {
        MethodInfo onKeyDown = typeof(Control).GetMethod(
            "OnKeyDown",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not locate the protected Control.OnKeyDown method.");

        var args = new KeyEventArgs(keys);
        onKeyDown.Invoke(control, [args]);
        return args;
    }

    private static TControl? FindDescendant<TControl>(Control parent)
        where TControl : Control
    {
        foreach (Control child in parent.Controls)
        {
            if (child is TControl match)
            {
                return match;
            }

            TControl? descendant = FindDescendant<TControl>(child);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
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
