namespace Sasd.Ui.Sample.Utility;

/// <summary>
/// Small, non-sensitive UI preference set persisted by the Utility reference application.
/// </summary>
/// <remarks>
/// The sample intentionally does not persist clipboard text, selected paths or other
/// arbitrary user content. The UI-state store is for disposable presentation state,
/// not a substitute for application/business-data persistence.
/// </remarks>
internal sealed record UtilityUiSettings(bool TrayVisible);
