namespace Sasd.Ui.Sample.Crud;

/// <summary>
/// Small application-owned model used by the CRUD reference sample.
/// </summary>
/// <remarks>
/// This type deliberately lives in the sample rather than the UI platform. It demonstrates that
/// domain/application data stays outside reusable UI packages even when the controls display and edit it.
/// </remarks>
internal sealed class CustomerRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Status { get; set; } = "Active";
}
