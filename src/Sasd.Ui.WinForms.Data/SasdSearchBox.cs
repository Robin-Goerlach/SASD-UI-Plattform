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

        searchTextBox = new TextBox
        {
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            PlaceholderText = "Search...",
        };
        searchTextBox.TextChanged += HandleTextChanged;

        clearButton = new Button
        {
            AccessibleName = "Clear search",
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.System,
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

    /// <summary>Focuses the text editor.</summary>
    public void FocusSearch() => searchTextBox.Focus();

    private void HandleTextChanged(object? sender, EventArgs e)
    {
        clearButton.Visible = searchTextBox.TextLength > 0;
        SearchTextChanged?.Invoke(this, EventArgs.Empty);
    }
}
