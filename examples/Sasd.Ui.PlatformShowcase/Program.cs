namespace Sasd.Ui.PlatformShowcase;

internal static class Program
{
    /// <summary>Starts the integrated SASD UI Platform showcase.</summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ShowcaseForm());
    }
}
