using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.State;

namespace Sasd.Ui.PlatformShowcase;

/// <summary>Demonstrates versioned UI-state storage and bounded recent-item references.</summary>
internal sealed class StatePage : UserControl
{
    private const string NotesStateKey = "showcase:notes";

    private readonly SasdStateStore stateStore;
    private readonly SasdRecentItemsService recentItemsService;
    private readonly Action<string, SasdStatusSeverity, TimeSpan?> publishStatus;
    private readonly TextBox notesTextBox = new();
    private readonly ListBox recentItemsList = new();
    private int recentCounter;

    public StatePage(
        SasdStateStore stateStore,
        SasdRecentItemsService recentItemsService,
        Action<string, SasdStatusSeverity, TimeSpan?> publishStatus)
    {
        this.stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        this.recentItemsService = recentItemsService ?? throw new ArgumentNullException(nameof(recentItemsService));
        this.publishStatus = publishStatus ?? throw new ArgumentNullException(nameof(publishStatus));

        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        Padding = new Padding(20);

        notesTextBox.AcceptsReturn = true;
        notesTextBox.AccessibleName = "Persisted example notes";
        notesTextBox.Dock = DockStyle.Fill;
        notesTextBox.Multiline = true;
        notesTextBox.ScrollBars = ScrollBars.Vertical;
        notesTextBox.Text = "Change this text, save it, change it again, then load the saved value.";

        recentItemsList.Dock = DockStyle.Fill;
        recentItemsList.HorizontalScrollbar = true;

        var noteButtons = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
        };
        noteButtons.Controls.Add(CreateAsyncButton("Save notes", SaveNotesAsync));
        noteButtons.Controls.Add(CreateAsyncButton("Load notes", LoadNotesAsync));
        noteButtons.Controls.Add(CreateAsyncButton("Remove notes state", RemoveNotesAsync));

        var recentButtons = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
        };
        recentButtons.Controls.Add(CreateAsyncButton("Add recent item", AddRecentItemAsync));
        recentButtons.Controls.Add(CreateAsyncButton("Reload recent items", LoadRecentItemsAsync));
        recentButtons.Controls.Add(CreateAsyncButton("Clear recent items", ClearRecentItemsAsync));

        var heading = new Label
        {
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Text =
                "UI state and recent items\r\n\r\n" +
                "The state store persists disposable UI state as versioned JSON under LocalAppData. " +
                "It is not a business-data database. The recent-items service stores only application-owned references and display names.",
        };

        var left = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(0, 8, 8, 0),
        };
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.Controls.Add(new Label { AutoSize = true, Text = "Persisted notes" }, 0, 0);
        left.Controls.Add(notesTextBox, 0, 1);
        left.Controls.Add(noteButtons, 0, 2);

        var right = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3,
            Padding = new Padding(8, 8, 0, 0),
        };
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.Controls.Add(new Label { AutoSize = true, Text = "Recent item references" }, 0, 0);
        right.Controls.Add(recentItemsList, 0, 1);
        right.Controls.Add(recentButtons, 0, 2);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 520,
        };
        split.Panel1.Controls.Add(left);
        split.Panel2.Controls.Add(right);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(split, 0, 1);
        Controls.Add(layout);

        Load += OnPageLoad;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Load -= OnPageLoad;
        }

        base.Dispose(disposing);
    }

    private async void OnPageLoad(object? sender, EventArgs e) => await LoadRecentItemsAsync();

    private async Task SaveNotesAsync()
    {
        await stateStore.SaveAsync(
            NotesStateKey,
            new DemoUiState(notesTextBox.Text, DateTimeOffset.UtcNow));
        publishStatus("Showcase notes saved to UI state.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(4));
    }

    private async Task LoadNotesAsync()
    {
        DemoUiState? state = await stateStore.LoadAsync<DemoUiState>(NotesStateKey);
        if (state is null)
        {
            publishStatus("No saved note state exists yet.", SasdStatusSeverity.Warning, TimeSpan.FromSeconds(4));
            return;
        }

        notesTextBox.Text = state.Notes;
        publishStatus(
            $"Showcase notes loaded (saved {state.SavedAtUtc.LocalDateTime:g}).",
            SasdStatusSeverity.Success,
            TimeSpan.FromSeconds(5));
    }

    private async Task RemoveNotesAsync()
    {
        bool removed = await stateStore.RemoveAsync(NotesStateKey);
        publishStatus(
            removed ? "Saved note state removed." : "No saved note state was present.",
            removed ? SasdStatusSeverity.Success : SasdStatusSeverity.Information,
            TimeSpan.FromSeconds(4));
    }

    private async Task AddRecentItemAsync()
    {
        recentCounter++;
        string reference = $"showcase://item/{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}-{recentCounter}";
        await recentItemsService.AddAsync(reference, $"Example item {recentCounter}");
        await LoadRecentItemsAsync();
        publishStatus("Recent item added.", SasdStatusSeverity.Success, TimeSpan.FromSeconds(3));
    }

    private async Task LoadRecentItemsAsync()
    {
        IReadOnlyList<SasdRecentItem> items = await recentItemsService.LoadAsync();
        recentItemsList.BeginUpdate();
        try
        {
            recentItemsList.Items.Clear();
            foreach (SasdRecentItem item in items)
            {
                recentItemsList.Items.Add($"{item.DisplayName ?? item.Reference} — {item.LastOpenedAtUtc.LocalDateTime:g}");
            }
        }
        finally
        {
            recentItemsList.EndUpdate();
        }
    }

    private async Task ClearRecentItemsAsync()
    {
        await recentItemsService.ClearAsync();
        await LoadRecentItemsAsync();
        publishStatus("Recent items cleared.", SasdStatusSeverity.Information, TimeSpan.FromSeconds(3));
    }

    private static Button CreateAsyncButton(string text, Func<Task> action)
    {
        var button = new Button
        {
            AutoSize = true,
            Margin = new Padding(0, 0, 8, 8),
            Text = text,
        };
        button.Click += async (_, _) =>
        {
            button.Enabled = false;
            try
            {
                await action();
            }
            finally
            {
                button.Enabled = true;
            }
        };
        return button;
    }
}
