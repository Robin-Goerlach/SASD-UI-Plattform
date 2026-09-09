namespace Sasd.Ui.WinForms.Data;

/// <summary>Persistable state for one data-grid column.</summary>
public sealed record SasdDataGridColumnState(
    string Id,
    int DisplayIndex,
    int Width,
    bool Visible);

/// <summary>Persistable, data-source-independent state of a SASD data grid.</summary>
public sealed record SasdDataGridState(IReadOnlyList<SasdDataGridColumnState> Columns)
{
    /// <summary>Captures visible layout properties using stable column identifiers.</summary>
    public static SasdDataGridState Capture(DataGridView grid)
    {
        ArgumentNullException.ThrowIfNull(grid);

        var columns = grid.Columns
            .Cast<DataGridViewColumn>()
            .Select(column => new
            {
                Column = column,
                Id = ResolveId(column),
            })
            .Where(item => item.Id is not null)
            .Select(item => new SasdDataGridColumnState(
                item.Id!,
                item.Column.DisplayIndex,
                item.Column.Width,
                item.Column.Visible))
            .OrderBy(item => item.DisplayIndex)
            .ToArray();

        return new SasdDataGridState(columns);
    }

    /// <summary>Restores known columns and safely ignores columns that no longer exist.</summary>
    public void Restore(DataGridView grid)
    {
        ArgumentNullException.ThrowIfNull(grid);

        var byId = grid.Columns
            .Cast<DataGridViewColumn>()
            .Select(column => new { Column = column, Id = ResolveId(column) })
            .Where(item => item.Id is not null)
            .ToDictionary(item => item.Id!, item => item.Column, StringComparer.Ordinal);

        foreach (var state in Columns)
        {
            if (!byId.TryGetValue(state.Id, out var column))
            {
                continue;
            }

            column.Visible = state.Visible;
            column.Width = Math.Clamp(state.Width, 20, 4000);
        }

        foreach (var state in Columns.OrderBy(item => item.DisplayIndex))
        {
            if (!byId.TryGetValue(state.Id, out var column))
            {
                continue;
            }

            column.DisplayIndex = Math.Clamp(state.DisplayIndex, 0, Math.Max(0, grid.Columns.Count - 1));
        }
    }

    private static string? ResolveId(DataGridViewColumn column)
    {
        if (!string.IsNullOrWhiteSpace(column.Name))
        {
            return column.Name;
        }

        return string.IsNullOrWhiteSpace(column.DataPropertyName) ? null : column.DataPropertyName;
    }
}
