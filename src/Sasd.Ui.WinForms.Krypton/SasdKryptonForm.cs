using System.ComponentModel;
using Krypton.Toolkit;

namespace Sasd.Ui.WinForms.Krypton;

/// <summary>
/// Krypton-backed SASD application window used by the R0.2 visual pilot.
///
/// This type deliberately lives in the Krypton adapter assembly. General SASD
/// contracts and native WinForms packages must never require <see cref="KryptonForm"/>
/// so applications can remain on the native implementation or replace Krypton later.
/// </summary>
public class SasdKryptonForm : KryptonForm
{
    /// <summary>Initialises a DPI-aware, keyboard-friendly Krypton form.</summary>
    public SasdKryptonForm()
    {
        // Keep the behavioural defaults aligned with SasdForm. They are repeated
        // here because C# cannot inherit from both SasdForm and KryptonForm.
        AutoScaleMode = AutoScaleMode.Dpi;
        KeyPreview = true;
        StartPosition = FormStartPosition.CenterParent;
    }

    /// <summary>
    /// Gets or sets the stable application-owned key used for safe window-state
    /// persistence. The state service remains vendor-neutral because a KryptonForm
    /// is still a normal WinForms Form.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(null)]
    [Description("Stable application-owned key for versioned UI-state persistence.")]
    public string? StateKey { get; set; }
}
