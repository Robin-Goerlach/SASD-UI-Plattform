namespace Sasd.Ui.ComponentGallery;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Start with the integration-oriented R1 shell. The existing detailed
        // component catalog remains available from the shell's toolbar/shortcut.
        Application.Run(new GalleryShellForm());
    }
}
