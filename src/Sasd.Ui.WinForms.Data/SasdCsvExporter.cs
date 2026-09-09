using System.Globalization;
using System.Text;

namespace Sasd.Ui.WinForms.Data;

/// <summary>Options for CSV export.</summary>
public sealed record SasdCsvExportOptions(
    char Delimiter = ',',
    bool IncludeHeaders = true,
    bool IncludeHiddenColumns = false,
    bool WriteUtf8Bom = true);

/// <summary>Exports DataGridView content without introducing an Excel dependency.</summary>
public static class SasdCsvExporter
{
    /// <summary>Exports the current grid rows to a stream.</summary>
    public static async Task ExportAsync(
        DataGridView grid,
        Stream destination,
        SasdCsvExportOptions? options = null,
        CultureInfo? culture = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(destination);

        options ??= new SasdCsvExportOptions();
        culture ??= CultureInfo.CurrentCulture;

        var columns = grid.Columns
            .Cast<DataGridViewColumn>()
            .Where(column => options.IncludeHiddenColumns || column.Visible)
            .OrderBy(column => column.DisplayIndex)
            .ToArray();

        var encoding = new UTF8Encoding(options.WriteUtf8Bom);
        await using var writer = new StreamWriter(destination, encoding, 4096, leaveOpen: true);

        if (options.IncludeHeaders)
        {
            await WriteRowAsync(
                writer,
                columns.Select(column => column.HeaderText),
                options.Delimiter,
                cancellationToken).ConfigureAwait(false);
        }

        foreach (DataGridViewRow row in grid.Rows)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (row.IsNewRow)
            {
                continue;
            }

            var values = columns.Select(column => Convert.ToString(
                row.Cells[column.Index].FormattedValue,
                culture) ?? string.Empty);

            await WriteRowAsync(writer, values, options.Delimiter, cancellationToken).ConfigureAwait(false);
        }

        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task WriteRowAsync(
        TextWriter writer,
        IEnumerable<string> values,
        char delimiter,
        CancellationToken cancellationToken)
    {
        var first = true;
        foreach (var value in values)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!first)
            {
                await writer.WriteAsync(delimiter).ConfigureAwait(false);
            }

            await writer.WriteAsync(Escape(value, delimiter)).ConfigureAwait(false);
            first = false;
        }

        await writer.WriteLineAsync().ConfigureAwait(false);
    }

    private static string Escape(string value, char delimiter)
    {
        if (value.IndexOfAny([delimiter, '"', '\r', '\n']) < 0)
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }
}
