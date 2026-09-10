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
