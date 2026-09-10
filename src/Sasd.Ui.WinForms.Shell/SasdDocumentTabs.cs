using System.ComponentModel;

namespace Sasd.Ui.WinForms.Shell;

/// <summary>Event data for a document managed by <see cref="SasdDocumentTabs"/>.</summary>
public sealed class SasdDocumentEventArgs : EventArgs
{
    /// <summary>Initialises document event data.</summary>
    public SasdDocumentEventArgs(string documentId, string title, Control content)
    {
        DocumentId = documentId;
        Title = title;
        Content = content;
    }

    /// <summary>Gets the stable application-owned document id.</summary>
    public string DocumentId { get; }

    /// <summary>Gets the current document title.</summary>
    public string Title { get; }

    /// <summary>Gets the document content control.</summary>
    public Control Content { get; }
}

/// <summary>
/// Provides a small document-tab host with stable ids, keyboard switching and predictable disposal.
/// </summary>
/// <remarks>
/// The host owns controls returned by the content factory. Closing a document
/// disposes its page and therefore its content. Applications should keep business
/// state outside the visual control if it must survive a close/reopen cycle.
/// </remarks>
public class SasdDocumentTabs : UserControl
{
    private readonly TabControl tabControl;
    private readonly Dictionary<string, DocumentEntry> documents = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Initialises the document-tab host.</summary>
    public SasdDocumentTabs()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Documents";
        AccessibleDescription = "Open application documents arranged in tabs.";

        tabControl = new TabControl
        {
            AccessibleName = "Document tabs",
            AccessibleRole = AccessibleRole.PageTabList,
            Dock = DockStyle.Fill,
            Multiline = false,
        };
        tabControl.SelectedIndexChanged += HandleSelectedIndexChanged;
        Controls.Add(tabControl);
    }

    /// <summary>Occurs after a new document has been created.</summary>
    public event EventHandler<SasdDocumentEventArgs>? DocumentOpened;

    /// <summary>Occurs after a document has been closed and removed.</summary>
    public event EventHandler<SasdDocumentEventArgs>? DocumentClosed;

    /// <summary>Occurs when the selected document changes.</summary>
    public event EventHandler<SasdDocumentEventArgs>? DocumentSelected;

    /// <summary>Gets the number of open documents.</summary>
    [Browsable(false)]
    public int DocumentCount => documents.Count;

    /// <summary>Gets the selected document id, or <see langword="null"/> when no document is open.</summary>
    [Browsable(false)]
    public string? SelectedDocumentId => FindEntry(tabControl.SelectedTab)?.Id;

    /// <summary>
    /// Opens a new document or selects the existing document with the same id.
    /// The returned control is the content instance owned by this host.
    /// </summary>
    public Control OpenOrSelect(string documentId, string title, Func<Control> contentFactory)
    {
        ValidateDocumentIdentity(documentId, title);
        ArgumentNullException.ThrowIfNull(contentFactory);

        if (documents.TryGetValue(documentId, out DocumentEntry? existing))
        {
            // The visible and accessible titles are one semantic value. Keeping both in sync is
            // important when an application reopens an already-present document under a newly
            // resolved title (for example after a rename in a tree/workbench surface).
            existing.Page.Text = title;
            existing.Page.AccessibleName = title;
            tabControl.SelectedTab = existing.Page;
            return existing.Content;
        }

        Control content = contentFactory() ?? throw new InvalidOperationException("The document content factory returned null.");
        content.Dock = DockStyle.Fill;

        var page = new TabPage(title)
        {
            AccessibleName = title,
            Padding = Padding.Empty,
        };
        page.Controls.Add(content);

        var entry = new DocumentEntry(documentId, page, content);
        documents.Add(documentId, entry);
        tabControl.TabPages.Add(page);
        tabControl.SelectedTab = page;
        DocumentOpened?.Invoke(this, CreateEventArgs(entry));
        return content;
    }

    /// <summary>Selects an existing document.</summary>
    public bool SelectDocument(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        if (!documents.TryGetValue(documentId, out DocumentEntry? entry))
        {
            return false;
        }

        tabControl.SelectedTab = entry.Page;
        return true;
    }

    /// <summary>Updates the visible and accessible title of an existing document.</summary>
    public bool SetDocumentTitle(string documentId, string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (!documents.TryGetValue(documentId, out DocumentEntry? entry))
        {
            return false;
        }

        entry.Page.Text = title;
        entry.Page.AccessibleName = title;
        return true;
    }

    /// <summary>Closes and disposes one document.</summary>
    public bool CloseDocument(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        if (!documents.Remove(documentId, out DocumentEntry? entry))
        {
            return false;
        }

        var eventArgs = CreateEventArgs(entry);
        tabControl.TabPages.Remove(entry.Page);
        entry.Page.Dispose();
        DocumentClosed?.Invoke(this, eventArgs);
        return true;
    }

    /// <summary>Closes and disposes all open documents.</summary>
    public void CloseAllDocuments()
    {
        foreach (string documentId in documents.Keys.ToArray())
        {
            CloseDocument(documentId);
        }
    }

    /// <inheritdoc />
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Keys keyCode = keyData & Keys.KeyCode;
        Keys modifiers = keyData & Keys.Modifiers;
        if (keyCode == Keys.Tab &&
            (modifiers == Keys.Control || modifiers == (Keys.Control | Keys.Shift)))
        {
            bool moveForward = modifiers == Keys.Control;
            if (SelectAdjacentDocument(moveForward))
            {
                return true;
            }
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Close explicitly so the dictionary never retains disposed content if
            // the host itself is disposed while documents are still open.
            CloseAllDocuments();
        }

        base.Dispose(disposing);
    }

    private bool SelectAdjacentDocument(bool moveForward)
    {
        if (tabControl.TabCount <= 1)
        {
            // Do not consume Ctrl+Tab when there is nothing to switch. A surrounding host may
            // legitimately use the key combination for a broader navigation surface.
            return false;
        }

        int currentIndex = Math.Max(0, tabControl.SelectedIndex);
        int offset = moveForward ? 1 : -1;
        int nextIndex = (currentIndex + offset + tabControl.TabCount) % tabControl.TabCount;
        tabControl.SelectedIndex = nextIndex;
        return true;
    }

    private void HandleSelectedIndexChanged(object? sender, EventArgs e)
    {
        DocumentEntry? entry = FindEntry(tabControl.SelectedTab);
        if (entry is not null)
        {
            DocumentSelected?.Invoke(this, CreateEventArgs(entry));
        }
    }

    private DocumentEntry? FindEntry(TabPage? page)
    {
        if (page is null)
        {
            return null;
        }

        return documents.Values.FirstOrDefault(entry => ReferenceEquals(entry.Page, page));
    }

    private static SasdDocumentEventArgs CreateEventArgs(DocumentEntry entry) =>
        new(entry.Id, entry.Page.Text, entry.Content);

    private static void ValidateDocumentIdentity(string documentId, string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (documentId.Length > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(documentId), "Document ids may contain at most 200 characters.");
        }
    }

    private sealed record DocumentEntry(string Id, TabPage Page, Control Content);
}
