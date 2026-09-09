namespace Sasd.Ui.Sample.Workbench;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new WorkbenchSampleForm());
    }
}
