using System.ComponentModel;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Small in-memory row used by the data-page examples.</summary>
internal sealed class DemoCustomer
{
    public DemoCustomer(string name, string email, string status, int score)
    {
        Name = name;
        Email = email;
        Status = status;
        Score = score;
    }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Status { get; set; }

    public int Score { get; set; }
}

/// <summary>
/// Example object edited by <c>SasdPropertyEditor</c>.
/// Attributes are intentionally included so the showcase demonstrates categories,
/// descriptions and hidden implementation properties rather than only primitive values.
/// </summary>
internal sealed class DemoSettings
{
    [Category("General")]
    [Description("Friendly name shown by the example application.")]
    public string ApplicationName { get; set; } = "SASD UI Platform Showcase";

    [Category("General")]
    [Description("Whether the example should display detailed diagnostic text.")]
    public bool ShowDiagnostics { get; set; } = true;

    [Category("Network")]
    [Description("Example endpoint. The showcase never connects to this address.")]
    public string ServiceEndpoint { get; set; } = "https://example.invalid/api";

    [Category("Network")]
    [Description("Example timeout used only to demonstrate numeric editing.")]
    public int TimeoutSeconds { get; set; } = 30;

    [Browsable(false)]
    public Guid InternalId { get; } = Guid.NewGuid();
}

/// <summary>Small state payload persisted by the state demonstration page.</summary>
internal sealed record DemoUiState(string Notes, DateTimeOffset SavedAtUtc);
