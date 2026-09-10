using System.ComponentModel;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Provides a compact search editor with a built-in clear action.</summary>
[DefaultEvent(nameof(SearchTextChanged))]
public class SasdSearchBox : UserControl
{
    private readonly TextBox searchTextBox;
    private readonly Button clearButton;

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
        AccessibleDescription = "Search text with a clear action.";

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
            AccessibleDescription = "Clears the current search text.",
            AccessibleName = "Clear search",
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.System,
            TabIndex = 1,
            TabStop = true,
            Text = "×",
            Width = 32,
            Visible = false,
        };
        clearButton.Click += (_, _) => searchTextBox.Clear();

        Controls.Add(searchTextBox);
        Controls.Add(clearButton);
    }

    /// <summary>Occurs when the search text changes.</summary>
    public event EventHandler? SearchTextChanged;

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

    /// <summary>Focuses the native search text editor.</summary>
    /// <remarks>
    /// Focus remains within the normal WinForms tab order. When search text is present, the
    /// clear button is the next focusable child so keyboard users can reach the same action
    /// that pointer users see.
    /// </remarks>
    public void FocusSearch() => searchTextBox.Focus();

    private void HandleTextChanged(object? sender, EventArgs e)
    {
        // Hiding the clear button while the editor is empty also removes an action that has
        // nothing to do from keyboard navigation and from the active accessibility tree.
        clearButton.Visible = searchTextBox.TextLength > 0;
        SearchTextChanged?.Invoke(this, EventArgs.Empty);
    }
}
