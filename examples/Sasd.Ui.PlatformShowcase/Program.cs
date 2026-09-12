namespace Sasd.Ui.PlatformShowcase;

internal static class Program
{
    private static readonly string[] SmokePageIds =
    [
        "overview",
        "forms",
        "data",
        "controls",
        "feedback",
        "state",
        "windows",
        "advanced",
        "acceptance",
        "self-test",
    ];

    /// <summary>Starts the integrated SASD UI Platform showcase.</summary>
    [STAThread]
    private static int Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Length == 1 && string.Equals(args[0], "--smoke", StringComparison.OrdinalIgnoreCase))
        {
            return RunNavigationSmoke();
        }

        Application.Run(new ShowcaseForm());
        return 0;
    }

    private static int RunNavigationSmoke()
    {
        try
        {
            using var form = new ShowcaseForm
            {
                Location = new Point(-32000, -32000),
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
            };

            // Show the shell off-screen so each lazily created page receives the same normal
            // WinForms handle/layout lifecycle as the interactive application. Navigation itself
            // is synchronous, so no DoEvents/Sleep polling is needed and the CI desktop remains
            // untouched by process-global input automation.
            form.Show();
            foreach (string pageId in SmokePageIds)
            {
                if (!form.NavigationHost.Navigate(pageId))
                {
                    throw new InvalidOperationException($"Showcase smoke could not navigate to registered page '{pageId}'.");
                }
            }

            form.Hide();
            Console.WriteLine($"SASD integrated showcase navigation smoke passed ({SmokePageIds.Length} pages).");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
