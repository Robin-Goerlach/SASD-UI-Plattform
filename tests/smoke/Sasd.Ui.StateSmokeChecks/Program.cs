using System.Text.Json;
using Sasd.Ui.WinForms;
using Sasd.Ui.WinForms.State;

namespace Sasd.Ui.StateSmokeChecks;

internal static class Program
{
    [STAThread]
    private static async Task<int> Main()
    {
        var root = Path.Combine(Path.GetTempPath(), "sasd-ui-state-smoke", Guid.NewGuid().ToString("N"));

        try
        {
            await ValidatePersistenceAndRecoveryAsync(root);
            await ValidateIncrementalMigrationAsync(root);
            await ValidateRecentItemsAsync(root);
            await RecentItemsPinningChecks.RunAsync(root);

            // The preceding file/state checks are intentionally asynchronous and may resume
            // on a pool thread because a console smoke executable has no UI message loop.
            // Form-state behavior, however, must run on a real STA WinForms context: the
            // production RestoreAsync method deliberately preserves its UI synchronization
            // context before touching the Form. Use a small invisible ApplicationContext
            // rather than weakening product ConfigureAwait behavior or calling DoEvents.
            await RunInStaMessageLoopAsync(() => ValidateFormStateAsync(root));

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

    private static async Task ValidatePersistenceAndRecoveryAsync(string root)
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
    }

    private static async Task ValidateIncrementalMigrationAsync(string root)
    {
        string migrationRoot = Path.Combine(root, "migration");
        Directory.CreateDirectory(migrationRoot);
        string statePath = Path.Combine(migrationRoot, "ui-state.json");

        // Simulate a document produced by schema v1. A real application migration
        // normally adjusts only the sections it owns and leaves unrelated sections intact.
        var legacyValue = JsonSerializer.SerializeToElement(new DemoState("Light", 7));
        var legacyDocument = new
        {
            schemaVersion = 1,
            updatedAtUtc = DateTimeOffset.UtcNow,
            sections = new Dictionary<string, JsonElement>
            {
                ["legacy-demo"] = legacyValue,
            },
        };
        await File.WriteAllTextAsync(statePath, JsonSerializer.Serialize(legacyDocument));

        var migration = new SasdStateMigration(1, 2, sections =>
        {
            if (sections.Remove("legacy-demo", out JsonElement value))
            {
                sections["demo"] = value;
            }
        });

        await using var store = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "MigrationSmokeChecks",
            migrationRoot,
            SchemaVersion: 2,
            Migrations: [migration]));

        var migrated = await store.LoadAsync<DemoState>("demo");
        Ensure(migrated == new DemoState("Light", 7), "Incremental UI-state migration did not run.");

        // Saving after a successful in-memory migration writes the current schema.
        await store.SaveAsync("demo", migrated);
        using JsonDocument persisted = JsonDocument.Parse(await File.ReadAllTextAsync(statePath));
        Ensure(persisted.RootElement.GetProperty("schemaVersion").GetInt32() == 2,
            "Migrated UI state was not persisted with the current schema version.");
    }

    private static async Task ValidateRecentItemsAsync(string root)
    {
        string recentRoot = Path.Combine(root, "recent-items");
        await using var store = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "RecentItemsSmokeChecks",
            recentRoot));
        var service = new SasdRecentItemsService(store, maximumItems: 2);

        // Seed deterministic timestamps through the underlying UI-state contract so this
        // test can prove sort/limit behavior without sleeps or assumptions about clock
        // resolution. The service is then exercised normally for mutation behavior.
        await store.SaveAsync("recent-items", new List<SasdRecentItem>
        {
            new("alpha", "Alpha", new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero)),
            new("beta", "Beta", new DateTimeOffset(2026, 1, 3, 8, 0, 0, TimeSpan.Zero)),
            new("gamma", "Gamma", new DateTimeOffset(2026, 1, 2, 8, 0, 0, TimeSpan.Zero)),
        });

        IReadOnlyList<SasdRecentItem> loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2, "Recent-items service did not enforce its maximum item count when loading.");
        Ensure(loaded[0].Reference == "beta" && loaded[1].Reference == "gamma",
            "Recent-items service did not order persisted entries newest first.");

        await service.AddAsync("gamma", "  Gamma renamed  ");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 && loaded[0].Reference == "gamma",
            "Adding an existing recent reference did not move it to the front without growing the list.");
        Ensure(loaded.Count(item => item.Reference == "gamma") == 1,
            "Recent-items service retained a duplicate reference after re-adding it.");
        Ensure(loaded[0].DisplayName == "Gamma renamed", "Recent-items display name was not normalized.");

        await service.AddAsync("delta");
        loaded = await service.LoadAsync();
        Ensure(loaded.Count == 2 && loaded[0].Reference == "delta" && loaded[1].Reference == "gamma",
            "Recent-items service did not trim the oldest item after reaching its limit.");
        Ensure(loaded[0].DisplayName is null, "Blank/omitted recent-item display name was not normalized to null.");

        Ensure(!await service.RemoveAsync("missing"), "Recent-items service reported a missing reference as removed.");
        Ensure(await service.RemoveAsync("gamma"), "Recent-items service could not remove an existing reference.");
        Ensure((await service.LoadAsync()).Single().Reference == "delta",
            "Recent-items removal changed the wrong persisted item.");

        await service.ClearAsync();
        Ensure((await service.LoadAsync()).Count == 0, "Recent-items service did not clear its state section.");
    }

    private static async Task ValidateFormStateAsync(string root)
    {
        string formRoot = Path.Combine(root, "form-state");
        await using var store = new SasdStateStore(new SasdStateStoreOptions(
            "SASD-GmbH",
            "FormStateSmokeChecks",
            formRoot));
        var service = new SasdFormStateService(store);

        using (var anonymousForm = new SasdForm())
        {
            await service.SaveAsync(anonymousForm);
            Ensure(!File.Exists(store.StatePath),
                "Form-state service persisted a form that intentionally had no StateKey.");
        }

        Rectangle workingArea = Screen.PrimaryScreen?.WorkingArea ?? SystemInformation.WorkingArea;
        var expectedBounds = new Rectangle(
            workingArea.Left + 24,
            workingArea.Top + 24,
            Math.Min(640, Math.Max(320, workingArea.Width - 48)),
            Math.Min(480, Math.Max(200, workingArea.Height - 48)));

        using var form = new SasdForm
        {
            StateKey = "main-window",
            StartPosition = FormStartPosition.Manual,
            Bounds = expectedBounds,
            WindowState = FormWindowState.Normal,
        };

        await service.SaveAsync(form);
        SasdWindowState persisted = await store.LoadAsync<SasdWindowState>("window:main-window")
            ?? throw new InvalidOperationException("Form-state service did not persist a keyed form.");
        Ensure(persisted.X == expectedBounds.X && persisted.Y == expectedBounds.Y &&
               persisted.Width == expectedBounds.Width && persisted.Height == expectedBounds.Height,
            "Form-state service persisted unexpected normal window bounds.");

        form.Bounds = new Rectangle(workingArea.Left + 80, workingArea.Top + 80, 360, 240);
        await service.RestoreAsync(form);
        Ensure(form.StartPosition == FormStartPosition.Manual,
            "Form-state restore did not switch the form to manual placement.");
        Ensure(form.Bounds == expectedBounds,
            "Form-state restore did not reapply the previously saved visible bounds.");

        // Off-screen state can occur after a monitor is removed. Test the safety helper
        // directly with deliberately impossible coordinates; exact centering is an OS
        // detail, but the restored rectangle must intersect the current working area.
        var offScreen = new SasdWindowState(int.MaxValue / 2, int.MaxValue / 2, 640, 480, FormWindowState.Normal);
        offScreen.Restore(form);
        Rectangle intersection = Rectangle.Intersect(form.Bounds, workingArea);
        Ensure(intersection.Width >= 80 && intersection.Height >= 40,
            "Window-state safety did not bring an off-screen window back onto the current desktop.");
    }

    private static Task RunInStaMessageLoopAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var thread = new Thread(() =>
        {
            using var context = new ApplicationContext();
            bool started = false;
            EventHandler? idleHandler = null;

            idleHandler = async (_, _) =>
            {
                if (started)
                {
                    return;
                }

                started = true;
                Application.Idle -= idleHandler;
                try
                {
                    // Application.Run has now installed the WinForms message-loop context.
                    // Awaiting here is intentional: production continuations that require the
                    // UI context can marshal back naturally while the invisible loop keeps pumping.
                    await action();
                    completion.TrySetResult();
                }
                catch (Exception exception)
                {
                    completion.TrySetException(exception);
                }
                finally
                {
                    context.ExitThread();
                }
            };

            Application.Idle += idleHandler;
            try
            {
                Application.Run(context);
            }
            catch (Exception exception)
            {
                completion.TrySetException(exception);
            }
            finally
            {
                Application.Idle -= idleHandler;
            }
        })
        {
            IsBackground = true,
            Name = "SASD UI state smoke STA",
        };

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return completion.Task;
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
