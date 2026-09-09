namespace Sasd.Ui.WinForms.Shell;

/// <summary>Event data raised before an application document tab is closed.</summary>
public sealed class SasdDocumentClosingEventArgs : EventArgs
{
    /// <summary>Initialises closing event data.</summary>
    public SasdDocumentClosingEventArgs(string documentId, TabPage page)
    {
        DocumentId = documentId;
        Page = page;
    }

    /// <summary>Gets the stable application-owned document identifier.</summary>
    public string DocumentId { get; }

    /// <summary>Gets the page that is about to close.</summary>
    public TabPage Page { get; }

    /// <summary>Gets or sets whether the application vetoes closing, for example because of unsaved work.</summary>
    public bool Cancel { get; set; }
}

/// <summary>
/// Manages document-style tabs by stable application identifiers. It intentionally
/// does not implement persistence or unsaved-work policy; those concerns are
/// supplied by the application through state services and <see cref="DocumentClosing"/>.
/// </summary>
public class SasdDocumentTabs : TabControl
{
    private readonly Dictionary<string, TabPage> pages = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Initialises document tabs.</summary>
    public SasdDocumentTabs()
    {
        HotTrack = true;
        Multiline = false;
        SizeMode = TabSizeMode.Normal;
    }

    /// <summary>Raised before a document is closed and may be cancelled by the application.</summary>
    public event EventHandler<SasdDocumentClosingEventArgs>? DocumentClosing;

    /// <summary>
    /// Opens a new document or activates the existing page with the same stable ID.
    /// The content factory runs only when a new page is required.
    /// </summary>
    public TabPage OpenOrSelect(string documentId, string title, Func<Control> contentFactory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(contentFactory);

        if (pages.TryGetValue(documentId, out TabPage? existing))
        {
            existing.Text = title;
            SelectedTab = existing;
            return existing;
        }

        Control content = contentFactory() ?? throw new InvalidOperationException("The document content factory returned null.");
        var page = new TabPage(title)
        {
            AccessibleName = title,
        };
        content.Dock = DockStyle.Fill;
        page.Controls.Add(content);
        pages.Add(documentId, page);
        TabPages.Add(page);
        SelectedTab = page;
        return page;
    }

    /// <summary>Attempts to close a document. Returns false when it is unknown or closing was cancelled.</summary>
    public bool TryClose(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        if (!pages.TryGetValue(documentId, out TabPage? page))
        {
            return false;
        }

        var eventArgs = new SasdDocumentClosingEventArgs(documentId, page);
        DocumentClosing?.Invoke(this, eventArgs);
        if (eventArgs.Cancel)
        {
            return false;
        }

        pages.Remove(documentId);
        TabPages.Remove(page);
        page.Dispose();
        return true;
    }

    /// <summary>Gets the currently open stable document IDs.</summary>
    public IReadOnlyCollection<string> GetOpenDocumentIds() => pages.Keys.ToArray();

    /// <summary>Returns whether a document is currently open.</summary>
    public bool ContainsDocument(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        return pages.ContainsKey(documentId);
    }

    /// <inheritdoc />
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control is not TabPage removed)
        {
            return;
        }

        string? key = pages.FirstOrDefault(pair => ReferenceEquals(pair.Value, removed)).Key;
        if (key is not null)
        {
            pages.Remove(key);
        }
    }
}
