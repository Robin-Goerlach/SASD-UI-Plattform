namespace Sasd.Ui.Sample.Utility;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new UtilitySampleForm());
    }
}
