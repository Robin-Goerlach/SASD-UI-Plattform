namespace Sasd.Ui.WinForms.Forms;

/// <summary>Severity of a validation message.</summary>
public enum SasdValidationSeverity
{
    /// <summary>Informational guidance.</summary>
    Information,

    /// <summary>Warning that does not necessarily block an operation.</summary>
    Warning,

    /// <summary>Error that blocks the validated operation.</summary>
    Error,
}

/// <summary>One user-safe validation message associated with a field key.</summary>
public sealed record SasdValidationMessage(
    string FieldKey,
    string Message,
    SasdValidationSeverity Severity = SasdValidationSeverity.Error);

/// <summary>Aggregate result of a validation run.</summary>
public sealed record SasdValidationResult(IReadOnlyList<SasdValidationMessage> Messages)
{
    /// <summary>Gets whether no blocking errors were reported.</summary>
    public bool IsValid => Messages.All(message => message.Severity != SasdValidationSeverity.Error);

    /// <summary>Returns an empty successful result.</summary>
    public static SasdValidationResult Success { get; } = new(Array.Empty<SasdValidationMessage>());
}
