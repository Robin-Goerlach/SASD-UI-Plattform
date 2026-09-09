namespace Sasd.Ui.Sample.Workbench;

/// <summary>
/// Application-owned document state used by the Workbench reference sample.
/// </summary>
/// <remarks>
/// This type deliberately lives in the sample instead of the UI Platform. The platform
/// owns visual document hosts, but the consuming application owns document identity,
/// content, edit policy and persistence semantics.
/// </remarks>
internal sealed class WorkbenchDocument
{
    public WorkbenchDocument(string id, string title, string content, bool isEditable)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(content);

        Id = id;
        Title = title;
        Content = content;
        IsEditable = isEditable;
    }

    public string Id { get; }

    public string Title { get; }

    public string Content { get; set; }

    public bool IsEditable { get; }
}
