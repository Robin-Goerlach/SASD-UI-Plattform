using Sasd.Ui.WinForms;

namespace Sasd.Ui.WinForms.State;

/// <summary>Connects <see cref="SasdForm.StateKey"/> with the versioned state store.</summary>
public sealed class SasdFormStateService
{
    private const string WindowPrefix = "window:";
    private readonly SasdStateStore store;

    /// <summary>Initialises the form-state service.</summary>
    public SasdFormStateService(SasdStateStore store)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
    }

    /// <summary>Restores the form placement when a state key and saved state exist.</summary>
    public async Task RestoreAsync(SasdForm form, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(form);
        if (string.IsNullOrWhiteSpace(form.StateKey))
        {
            return;
        }

        var state = await store.LoadAsync<SasdWindowState>(WindowPrefix + form.StateKey, cancellationToken)
            .ConfigureAwait(true);
        state?.Restore(form);
    }

    /// <summary>Captures and persists the current safe form placement.</summary>
    public Task SaveAsync(SasdForm form, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(form);
        if (string.IsNullOrWhiteSpace(form.StateKey))
        {
            return Task.CompletedTask;
        }

        var state = SasdWindowState.Capture(form);
        return store.SaveAsync(WindowPrefix + form.StateKey, state, cancellationToken);
    }
}
