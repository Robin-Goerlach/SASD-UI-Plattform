namespace Sasd.Ui.Sample.Crud;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new CrudSampleForm());
    }
}
