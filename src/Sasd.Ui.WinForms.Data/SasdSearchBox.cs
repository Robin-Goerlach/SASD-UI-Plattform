using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Event data for a committed search request.</summary>
public sealed class SasdSearchRequestedEventArgs : EventArgs
{
    /// <summary>Initialises search-request event data.</summary>
    public SasdSearchRequestedEventArgs(string searchText) => SearchText = searchText;

    /// <summary>Gets the exact search text that was current when the request was raised.</summary>
    public string SearchText { get; }
}

/// <summary>
/// Provides a compact search editor with a built-in clear action and a UI-thread-owned
/// debounce for application search requests.
/// </summary>
[DefaultEvent(nameof(SearchTextChanged))]
public class SasdSearchBox : UserControl
{
    private const int DefaultDebounceMilliseconds = 300;

    private readonly TextBox searchTextBox;
    private readonly Button clearButton;
    private readonly System.Windows.Forms.Timer searchRequestTimer;
    private int debounceMilliseconds = DefaultDebounceMilliseconds;

    /// <summary>Initialises the search box.</summary>
    public SasdSearchBox()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(180, 30);
        Height = 30;

        // The composite itself is a useful UIA grouping, while keyboard focus remains on
        // its native editor/button children. Giving both children explicit names avoids
        // relying on placeholder text or the visual multiplication sign as accessibility
        // labels; those fallbacks are inconsistent between assistive-technology clients.
        AccessibleRole = AccessibleRole.Grouping;
        AccessibleName = "Search";
        AccessibleDescription = "Search text with a clear action. Press Enter to request the current search immediately.";

        searchTextBox = new TextBox
        {
            AccessibleName = "Search text",
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            PlaceholderText = "Search...",
            TabIndex = 0,
        };
        searchTextBox.TextChanged += HandleTextChanged;

        clearButton = new Button
        {
            AccessibleDescription = "Clears the current search text and requests updated results.",
            AccessibleName = "Clear search",
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.System,
            TabIndex = 1,
            TabStop = true,
            Text = "×",
            Width = 32,
            Visible = false,
        };
        clearButton.Click += HandleClearClicked;

        // A WinForms Timer executes on the owning UI message loop. Search debouncing therefore
        // does not introduce a worker thread, cross-thread control access or a process-wide
        // scheduler. The component creates this timer and owns/disposes it explicitly.
        searchRequestTimer = new System.Windows.Forms.Timer
        {
            Interval = DefaultDebounceMilliseconds,
        };
        searchRequestTimer.Tick += HandleSearchRequestTimerTick;

        Controls.Add(searchTextBox);
        Controls.Add(clearButton);
    }

    /// <summary>Occurs immediately whenever the editor text changes.</summary>
    /// <remarks>
    /// Use this event for UI state or presentation that must follow each edit. Applications
    /// performing actual search work should normally use <see cref="SearchRequested"/> so
    /// rapid typing does not start one data operation per keystroke.
    /// </remarks>
    public event EventHandler? SearchTextChanged;

    /// <summary>
    /// Occurs after the search text has remained unchanged for <see cref="DebounceMilliseconds"/>,
    /// or immediately when the user presses Enter, clears a non-empty search, or the application
    /// calls <see cref="RequestSearch"/>.
    /// </summary>
    public event EventHandler<SasdSearchRequestedEventArgs>? SearchRequested;

    /// <summary>Gets or sets the current search text.</summary>
    [Category("SASD")]
    [DefaultValue("")]
    public string SearchText
    {
        get => searchTextBox.Text;
        set => searchTextBox.Text = value ?? string.Empty;
    }

    /// <summary>Gets or sets the placeholder shown while the editor is empty.</summary>
    [Category("SASD")]
    [DefaultValue("Search...")]
    public string PlaceholderText
    {
        get => searchTextBox.PlaceholderText;
        set => searchTextBox.PlaceholderText = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the quiet period, in milliseconds, before an edited value raises
    /// <see cref="SearchRequested"/>. Set to zero to request on every text change.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(DefaultDebounceMilliseconds)]
    public int DebounceMilliseconds
    {
        get => debounceMilliseconds;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The search debounce cannot be negative.");
            }

            if (debounceMilliseconds == value)
            {
                return;
            }

            bool requestPending = searchRequestTimer.Enabled;
            searchRequestTimer.Stop();
            debounceMilliseconds = value;

            if (value > 0)
            {
                searchRequestTimer.Interval = value;
            }

            // Preserve a pending logical search when the application changes the debounce at
            // runtime. Moving to zero commits it now; another positive value restarts the quiet
            // period under the new policy instead of silently dropping the pending request.
            if (requestPending)
            {
                if (value == 0)
                {
                    RaiseSearchRequested();
                }
                else
                {
                    searchRequestTimer.Start();
                }
            }
        }
    }

    /// <summary>Focuses the native search text editor.</summary>
    /// <remarks>
    /// Focus remains within the normal WinForms tab order. When search text is present, the
    /// clear button is the next focusable child so keyboard users can reach the same action
    /// that pointer users see.
    /// </remarks>
    public void FocusSearch() => searchTextBox.Focus();

    /// <summary>
    /// Immediately raises <see cref="SearchRequested"/> for the current text and cancels any
    /// pending debounce for that same edit state.
    /// </summary>
    public void RequestSearch()
    {
        searchRequestTimer.Stop();
        RaiseSearchRequested();
    }

    /// <summary>
    /// Clears a non-empty search and immediately requests the resulting empty search state.
    /// A call while already empty is a no-op.
    /// </summary>
    public void ClearSearch()
    {
        if (searchTextBox.TextLength == 0)
        {
            return;
        }

        // With zero debounce the TextChanged path raises SearchRequested synchronously. For a
        // positive delay, replace the newly scheduled timer with one immediate empty-state
        // request so pointer and keyboard clearing never leave stale filtered results visible.
        bool textChangeRequestsImmediately = debounceMilliseconds == 0;
        searchTextBox.Clear();
        if (!textChangeRequestsImmediately)
        {
            RequestSearch();
        }
    }

    /// <inheritdoc />
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (searchTextBox.ContainsFocus && keyData == Keys.Enter)
        {
            RequestSearch();
            return true;
        }

        if (ContainsFocus && searchTextBox.TextLength > 0 && keyData == Keys.Escape)
        {
            ClearSearch();
            return true;
        }

        // An empty search deliberately does not claim Escape; a containing dialog remains free
        // to use its normal Cancel/Escape behavior when there is no search state to clear.
        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            searchRequestTimer.Stop();
            searchRequestTimer.Tick -= HandleSearchRequestTimerTick;
            searchRequestTimer.Dispose();
            searchTextBox.TextChanged -= HandleTextChanged;
            clearButton.Click -= HandleClearClicked;
        }

        base.Dispose(disposing);
    }

    private void HandleTextChanged(object? sender, EventArgs e)
    {
        // Hiding the clear button while the editor is empty also removes an action that has
        // nothing to do from keyboard navigation and from the active accessibility tree.
        clearButton.Visible = searchTextBox.TextLength > 0;
        SearchTextChanged?.Invoke(this, EventArgs.Empty);
        ScheduleSearchRequest();
    }

    private void HandleClearClicked(object? sender, EventArgs e) => ClearSearch();

    private void HandleSearchRequestTimerTick(object? sender, EventArgs e)
    {
        // WinForms Timer is periodic by default. Stop it before publishing so one quiet edit
        // produces exactly one logical request rather than repeated searches every interval.
        searchRequestTimer.Stop();
        RaiseSearchRequested();
    }

    private void ScheduleSearchRequest()
    {
        searchRequestTimer.Stop();
        if (debounceMilliseconds == 0)
        {
            RaiseSearchRequested();
            return;
        }

        searchRequestTimer.Interval = debounceMilliseconds;
        searchRequestTimer.Start();
    }

    private void RaiseSearchRequested() =>
        SearchRequested?.Invoke(this, new SasdSearchRequestedEventArgs(searchTextBox.Text));
}
