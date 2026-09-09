namespace Sasd.Ui.WinForms.Forms;

/// <summary>
/// Coordinates synchronous and asynchronous form validation while presenting field errors through ErrorProvider.
/// </summary>
public sealed class SasdValidationCoordinator : IDisposable
{
    private readonly ErrorProvider errorProvider;
    private readonly List<RuleRegistration> rules = [];
    private CancellationTokenSource? activeValidation;
    private bool disposed;

    /// <summary>Initialises a validation coordinator for one container control.</summary>
    public SasdValidationCoordinator(ContainerControl container)
    {
        ArgumentNullException.ThrowIfNull(container);
        errorProvider = new ErrorProvider(container)
        {
            BlinkStyle = ErrorBlinkStyle.NeverBlink,
        };
    }

    /// <summary>Raised after a complete validation run.</summary>
    public event EventHandler<SasdValidationResult>? ValidationCompleted;

    /// <summary>Adds an asynchronous validation rule for one control.</summary>
    public void AddRule(
        string fieldKey,
        Control control,
        Func<CancellationToken, ValueTask<SasdValidationMessage?>> rule)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldKey);
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(rule);
        ThrowIfDisposed();
        rules.Add(new RuleRegistration(fieldKey, control, rule));
    }

    /// <summary>Adds a standard required-text rule.</summary>
    public void AddRequired(string fieldKey, TextBoxBase control, string fieldLabel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldLabel);
        AddRule(fieldKey, control, _ => ValueTask.FromResult<SasdValidationMessage?>(
            string.IsNullOrWhiteSpace(control.Text)
                ? new SasdValidationMessage(fieldKey, $"{fieldLabel} is required.")
                : null));
    }

    /// <summary>Runs all registered rules. A newer run cancels a stale run.</summary>
    public async Task<SasdValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        activeValidation?.Cancel();
        activeValidation?.Dispose();
        activeValidation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = activeValidation.Token;

        ClearErrors();
        var messages = new List<SasdValidationMessage>();

        foreach (var registration in rules)
        {
            token.ThrowIfCancellationRequested();
            var message = await registration.Rule(token).ConfigureAwait(true);
            if (message is null)
            {
                continue;
            }

            messages.Add(message);
            if (message.Severity == SasdValidationSeverity.Error)
            {
                errorProvider.SetError(registration.Control, message.Message);
            }
        }

        var result = messages.Count == 0
            ? SasdValidationResult.Success
            : new SasdValidationResult(messages);

        ValidationCompleted?.Invoke(this, result);
        return result;
    }

    /// <summary>Clears currently displayed validation errors.</summary>
    public void ClearErrors()
    {
        ThrowIfDisposed();
        foreach (var registration in rules)
        {
            errorProvider.SetError(registration.Control, string.Empty);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        activeValidation?.Cancel();
        activeValidation?.Dispose();
        errorProvider.Dispose();
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
    }

    private sealed record RuleRegistration(
        string FieldKey,
        Control Control,
        Func<CancellationToken, ValueTask<SasdValidationMessage?>> Rule);
}
