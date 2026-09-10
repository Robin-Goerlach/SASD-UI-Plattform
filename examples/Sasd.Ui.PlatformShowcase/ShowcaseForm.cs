using Sasd.Ui.Core;
using Sasd.Ui.WinForms.Commands;
using Sasd.Ui.WinForms.Dialogs;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;
using Sasd.Ui.WinForms.Theming;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>
/// Main window for the integrated platform showcase.
/// </summary>
/// <remarks>
/// <para>
/// The form intentionally owns the application-level services and passes them to pages. This
/// demonstrates the architecture expected from consuming applications: controls remain small,
/// services are explicit, and no global service locator is required.
/// </para>
/// <para>
/// The example keeps navigation and command registration in one place so readers can understand
/// the complete application composition without first learning a DI framework.
/// </para>
/// </remarks>
internal sealed class ShowcaseForm : SasdShellForm
{
    private readonly SasdThemeService themeService = new();
    private readonly SasdNotificationService notificationService = new();
    private readonly SasdDialogService dialogService = new();
    private readonly SasdFileDialogService fileDialogService = new();
    private readonly SasdClipboardService clipboardService = new();
    private readonly SasdDragDropService dragDropService = new();
    private readonly SasdStateStore stateStore;
    private readonly SasdRecentItemsService recentItemsService;

    /// <summary>Creates the showcase shell and registers all demonstration pages.</summary>
    public ShowcaseForm()
    {
        Text = "SASD UI Platform — Integrated Showcase";
        StateKey = "Example.PlatformShowcase.Main";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(980, 680);
        ClientSize = new Size(1280, 820);

        stateStore = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "Sasd.Ui.PlatformShowcase"));
        recentItemsService = new SasdRecentItemsService(stateStore);

        NavigationHost.CachePages = true;
        NavigationHost.NavigationWidth = 230;
        NavigationHost.RegisterPage("overview", "Overview", CreateOverviewPage);
        NavigationHost.RegisterPage("forms", "Forms & validation", CreateFormsPage);
        NavigationHost.RegisterPage("data", "Data & grid", CreateDataPage);
        NavigationHost.RegisterPage("controls", "Controls lab", CreateControlsLabPage);
        NavigationHost.RegisterPage("feedback", "Dialogs & feedback", CreateFeedbackPage);
        NavigationHost.RegisterPage("state", "State & recent items", CreateStatePage);
        NavigationHost.RegisterPage("windows", "Windows integration", CreateWindowsPage);
        NavigationHost.RegisterPage("advanced", "R2 native controls", CreateAdvancedPage);
        NavigationHost.RegisterPage("self-test", "Self test", CreateSelfTestPage);
        NavigationHost.Navigated += OnNavigated;

        RegisterShowcaseCommands();
        NavigationHost.Navigate("overview");
        themeService.Apply(this);
        PublishStatus("Integrated showcase ready.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigationHost.Navigated -= OnNavigated;

            // SasdStateStore currently has synchronous disposal work behind IAsyncDisposable.
            // Waiting here keeps ownership explicit and avoids leaking its synchronization gate.
            stateStore.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        base.Dispose(disposing);
    }

    private void RegisterShowcaseCommands()
    {
        RegisterCommand(CreateThemeCommand(
            "showcase.theme.light",
            "Light",
            SasdThemeCatalog.Light,
            Keys.Control | Keys.Alt | Keys.L));

        RegisterCommand(CreateThemeCommand(
            "showcase.theme.dark",
            "Dark",
            SasdThemeCatalog.Dark,
            Keys.Control | Keys.Alt | Keys.D));

        RegisterCommand(CreateThemeCommand(
            "showcase.theme.contrast",
            "High contrast",
            SasdThemeCatalog.HighContrast,
            Keys.Control | Keys.Alt | Keys.H));

        CommandBar.AddSeparator();
        RegisterCommand(new SasdCommand(
            "showcase.self-test",
            "Self test",
            _ =>
            {
                NavigationHost.Navigate("self-test");
                return Task.CompletedTask;
            },
            "Open the manual and automatic showcase test surface.",
            Keys.Control | Keys.T));
    }

    private SasdCommand CreateThemeCommand(
        string id,
        string text,
        SasdThemeDefinition theme,
        Keys shortcut) =>
        new(
            id,
            text,
            _ =>
            {
                themeService.SetThemeAndApply(theme, this);
                PublishStatus($"Theme changed to {theme.Mode}.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
                notificationService.Publish(
                    $"The showcase now uses the {theme.Mode} theme.",
                    "Theme changed",
                    SasdNotificationSeverity.Information,
                    TimeSpan.FromSeconds(4));
                return Task.CompletedTask;
            },
            $"Apply the built-in {theme.Mode} theme.",
            shortcut);

    private Control CreateOverviewPage() => new OverviewPage();

    private Control CreateFormsPage() => new FormsPage(PublishStatus);

    private Control CreateDataPage() => new DataPage(PublishStatus);

    private Control CreateControlsLabPage() => new ControlsLabPage(PublishStatus);

    private Control CreateFeedbackPage() =>
        new FeedbackPage(dialogService, notificationService, PublishStatus);

    private Control CreateStatePage() =>
        new StatePage(stateStore, recentItemsService, PublishStatus);

    private Control CreateWindowsPage() =>
        new WindowsPage(fileDialogService, clipboardService, dragDropService, PublishStatus);

    private Control CreateAdvancedPage() => new AdvancedPage(PublishStatus);

    private Control CreateSelfTestPage() =>
        new SelfTestPage(stateStore, themeService, PublishStatus);

    private void OnNavigated(object? sender, SasdNavigationEventArgs e)
    {
        // Cached pages can be created after a theme change. Re-applying the current theme to the
        // full shell keeps newly created controls visually consistent without global theme state.
        themeService.Apply(this);
        PublishStatus($"Page: {e.PageId}", SasdStatusSeverity.Information, TimeSpan.FromSeconds(2));
    }

    private void PublishStatus(
        string text,
        SasdStatusSeverity severity = SasdStatusSeverity.Information,
        TimeSpan? lifetime = null) =>
        StatusService.Publish(new SasdStatusMessage(text, severity, 0, lifetime));
}
