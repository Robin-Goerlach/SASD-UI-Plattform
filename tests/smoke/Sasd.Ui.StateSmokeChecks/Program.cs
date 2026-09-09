using Sasd.Ui.WinForms.State;

namespace Sasd.Ui.StateSmokeChecks;

internal static class Program
{
    private static async Task<int> Main()
    {
        var root = Path.Combine(Path.GetTempPath(), "sasd-ui-state-smoke", Guid.NewGuid().ToString("N"));

        try
        {
            await using var store = new SasdStateStore(new SasdStateStoreOptions(
                "SASD-GmbH",
                "StateSmokeChecks",
                root));

            var expected = new DemoState("Dark", 42);
            await store.SaveAsync("demo", expected);

            var actual = await store.LoadAsync<DemoState>("demo");
            Ensure(actual == expected, "Saved state could not be loaded.");
            Ensure(File.Exists(store.StatePath), "Primary state file was not created.");

            await store.SaveAsync("demo", expected with { Counter = 43 });
            var backupPath = Path.Combine(root, "ui-state.backup.json");
            Ensure(File.Exists(backupPath), "Backup state file was not created on replacement.");

            await File.WriteAllTextAsync(store.StatePath, "{ this is deliberately invalid json");
            var recovered = await store.LoadAsync<DemoState>("demo");
            Ensure(recovered == expected, "State store did not recover the previous valid backup.");

            Ensure(await store.RemoveAsync("demo"), "Existing section was not removed.");
            Ensure(await store.LoadAsync<DemoState>("demo") is null, "Removed section is still present.");

            await store.ResetAsync();
            Ensure(!File.Exists(store.StatePath), "Reset did not remove the primary state file.");
            Ensure(!File.Exists(backupPath), "Reset did not remove the backup state file.");

            Console.WriteLine("SASD UI state smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, true);
            }
        }
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed record DemoState(string Theme, int Counter);
}
