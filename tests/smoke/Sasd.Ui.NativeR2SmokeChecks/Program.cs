using System.ComponentModel;
using System.Reflection;
using Sasd.Ui.WinForms.Data;
using Sasd.Ui.WinForms.Forms;
using Sasd.Ui.WinForms.Media;

namespace Sasd.Ui.NativeR2SmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            ValidatePropertyEditor();
            ValidateGridColumnChooser();
            ValidateImageViewer();

            Console.WriteLine("SASD native R2 smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidatePropertyEditor()
    {
        using var editor = new SasdPropertyEditor();
        var settings = new ExampleSettings
        {
            Name = "Demo",
            RetryCount = 3,
            Endpoint = "https://example.test",
        };

        editor.SelectedObject = settings;
        Ensure(ReferenceEquals(editor.SelectedObject, settings),
            "Property editor did not retain the application-owned object reference.");

        PropertyGrid grid = GetPrivateField<PropertyGrid>(editor, "propertyGrid");
        PropertyDescriptorCollection initial = TypeDescriptor.GetProperties(grid.SelectedObject!);
        Ensure(initial.Cast<PropertyDescriptor>().Any(property => property.Name == nameof(ExampleSettings.Name)),
            "Property editor projection omitted a browsable property.");
        Ensure(initial.Cast<PropertyDescriptor>().All(property => property.Name != nameof(ExampleSettings.InternalId)),
            "Property editor projection exposed a Browsable(false) property.");

        editor.FilterText = "endpoint";
        PropertyDescriptorCollection filtered = TypeDescriptor.GetProperties(grid.SelectedObject!);
        Ensure(filtered.Count == 1 && filtered[0].Name == nameof(ExampleSettings.Endpoint),
            "Property editor filter did not reduce the descriptor projection predictably.");

        editor.ForceReadOnly = true;
        PropertyDescriptorCollection readOnly = TypeDescriptor.GetProperties(grid.SelectedObject!);
        Ensure(readOnly.Count == 1 && readOnly[0].IsReadOnly,
            "Forced read-only mode did not project a read-only descriptor.");

        editor.ClearFilter();
        Ensure(editor.FilterText.Length == 0, "Property editor did not clear its filter text.");
    }

    private static void ValidateGridColumnChooser()
    {
        using var grid = new SasdDataGrid
        {
            AutoGenerateColumns = false,
            FillAvailableWidth = false,
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Name" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Status" });

        using var chooser = new SasdGridColumnChooser();
        chooser.Bind(grid);
        Ensure(ReferenceEquals(chooser.Grid, grid), "Column chooser did not retain the bound grid reference.");

        CheckedListBox list = GetPrivateField<CheckedListBox>(chooser, "columnList");
        Ensure(list.Items.Count == 2, "Column chooser did not project both grid columns.");

        list.SetItemChecked(1, false);
        Ensure(!grid.Columns[1].Visible, "Column chooser did not hide the unchecked grid column.");

        chooser.ShowAllColumns();
        Ensure(grid.Columns.Cast<DataGridViewColumn>().All(column => column.Visible),
            "Column chooser did not restore all grid columns.");

        chooser.FilterText = "Name";
        Ensure(list.Items.Count == 1, "Column chooser filter did not reduce the visible chooser entries.");

        chooser.Unbind();
        Ensure(chooser.Grid is null && list.Items.Count == 0,
            "Column chooser did not release its grid binding cleanly.");
    }

    private static void ValidateImageViewer()
    {
        using var viewer = new SasdImageViewer();
        using var source = new Bitmap(32, 16);

        viewer.SetImage(source);
        Image displayedImage = viewer.Image
            ?? throw new InvalidOperationException("Image viewer did not retain an assigned image copy.");
        Ensure(viewer.HasImage, "Image viewer did not report its assigned image.");
        Ensure(!ReferenceEquals(source, displayedImage), "Image viewer retained the caller-owned Image instance instead of cloning it.");
        Ensure(displayedImage.Width == 32 && displayedImage.Height == 16,
            "Image viewer changed the assigned image dimensions unexpectedly.");

        viewer.ZoomMode = SasdImageZoomMode.ActualSize;
        Ensure(viewer.ZoomMode == SasdImageZoomMode.ActualSize, "Image viewer did not enter actual-size mode.");

        viewer.ZoomIn(25);
        Ensure(viewer.ZoomMode == SasdImageZoomMode.Custom && viewer.ZoomPercent == 125,
            "Image viewer did not switch to the requested custom zoom level.");

        viewer.ZoomOut(50);
        Ensure(viewer.ZoomPercent == 75, "Image viewer did not reduce custom zoom predictably.");

        viewer.ResetZoom();
        Ensure(viewer.ZoomMode == SasdImageZoomMode.Fit && viewer.ZoomPercent == 100,
            "Image viewer did not restore its default zoom state.");

        EnsureThrows<ArgumentOutOfRangeException>(
            () => viewer.ZoomPercent = 5,
            "Image viewer accepted an unsafe custom zoom below its documented range.");

        viewer.ClearImage();
        Ensure(!viewer.HasImage && viewer.Image is null, "Image viewer did not clear its owned image copy.");
    }

    private static T GetPrivateField<T>(object instance, string fieldName)
        where T : class
    {
        FieldInfo? field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (field?.GetValue(instance) is not T value)
        {
            throw new InvalidOperationException($"Expected private field '{fieldName}' was not available for smoke inspection.");
        }

        return value;
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

    private sealed class ExampleSettings
    {
        [Category("General")]
        [Description("User-facing name used by the example settings object.")]
        public string Name { get; set; } = string.Empty;

        [Category("General")]
        public int RetryCount { get; set; }

        [Category("Network")]
        [Description("Remote endpoint used by the example settings object.")]
        public string Endpoint { get; set; } = string.Empty;

        [Browsable(false)]
        public Guid InternalId { get; } = Guid.NewGuid();
    }
}
