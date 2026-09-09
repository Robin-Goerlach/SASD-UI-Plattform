namespace Sasd.Ui.Core;

/// <summary>
/// Represents the outcome of an expected UI or environment operation without
/// forcing the caller to use exceptions for recoverable failures.
/// </summary>
public sealed record UiOperationResult(
    bool Succeeded,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null,
    Exception? Exception = null)
{
    /// <summary>Creates a successful result.</summary>
    public static UiOperationResult Success() => new(true);

    /// <summary>Creates a failed result with a user-safe message.</summary>
    public static UiOperationResult Failure(
        string userMessage,
        string? technicalDetails = null,
        string? errorCode = null,
        Exception? exception = null) =>
        new(false, userMessage, technicalDetails, errorCode, exception);
}

/// <summary>
/// Represents the outcome of an operation that can return a value.
/// </summary>
/// <typeparam name="T">The returned value type.</typeparam>
public sealed record UiOperationResult<T>(
    bool Succeeded,
    T? Value = default,
    string? UserMessage = null,
    string? TechnicalDetails = null,
    string? ErrorCode = null)
{
    /// <summary>Creates a successful result containing <paramref name="value"/>.</summary>
    public static UiOperationResult<T> Success(T value) => new(true, value);

    /// <summary>Creates a failed result with a user-safe message.</summary>
    public static UiOperationResult<T> Failure(
        string userMessage,
        string? technicalDetails = null,
        string? errorCode = null) =>
        new(false, default, userMessage, technicalDetails, errorCode);
}
