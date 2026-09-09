using Krypton.Toolkit;
using Sasd.Ui.WinForms.Krypton;

namespace Sasd.Ui.KryptonSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        try
        {
            Ensure(KryptonAdapterStatus.IsImplemented, "Krypton adapter must report an implemented pilot.");
            Ensure(KryptonAdapterStatus.DecisionRecord == "ADR-0004", "Unexpected Krypton ADR reference.");

            using var form = new SasdKryptonForm
            {
                StateKey = "Smoke.KryptonForm",
                Text = "Krypton smoke form",
            };
            using var button = new KryptonButton { Text = "Smoke" };
            form.Controls.Add(button);

            Ensure(form is KryptonForm, "SasdKryptonForm must remain a KryptonForm.");
            Ensure(form.AutoScaleMode == AutoScaleMode.Dpi, "Krypton form must use DPI scaling.");
            Ensure(form.KeyPreview, "Krypton form must keep keyboard preview enabled.");
            Ensure(form.StateKey == "Smoke.KryptonForm", "Krypton state key was not retained.");

            // KryptonForm may route client controls through vendor-owned containers.
            // The smoke test deliberately checks only public, stable behavior and does
            // not depend on undocumented parent/FindForm relationships. Designer and
            // focus behavior are validated separately in the R0.2 UI matrix.
            Ensure(button.Parent is not null, "Krypton control was not attached to a client container.");
            Ensure(!string.IsNullOrWhiteSpace(KryptonAdapterStatus.RuntimeAssemblyVersion),
                "Krypton runtime assembly version must be discoverable.");

            Console.WriteLine($"Krypton R0.2 smoke checks passed. Runtime assembly: {KryptonAdapterStatus.RuntimeAssemblyVersion}");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
