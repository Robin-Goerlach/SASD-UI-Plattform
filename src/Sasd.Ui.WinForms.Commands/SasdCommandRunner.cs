using System.Collections.Concurrent;
using Sasd.Ui.Core;

namespace Sasd.Ui.WinForms.Commands;

/// <summary>Executes commands with predictable re-entry and recoverable-error semantics.</summary>
public sealed class SasdCommandRunner
{
    private readonly ConcurrentDictionary<string, byte> runningCommands = new(StringComparer.Ordinal);

    /// <summary>Executes a command and converts expected UI failures into a result object.</summary>
    public async Task<UiOperationResult> ExecuteAsync(
        SasdCommand command,
        bool preventReentry = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!command.Enabled)
        {
            return UiOperationResult.Failure("The command is currently unavailable.", errorCode: "COMMAND_DISABLED");
        }

        if (preventReentry && !runningCommands.TryAdd(command.Id, 0))
        {
            return UiOperationResult.Failure("The command is already running.", errorCode: "COMMAND_ALREADY_RUNNING");
        }

        try
        {
            await command.ExecuteCoreAsync(cancellationToken).ConfigureAwait(true);
            return UiOperationResult.Success();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return UiOperationResult.Failure("The operation was cancelled.", errorCode: "COMMAND_CANCELLED");
        }
        catch (Exception exception)
        {
            return UiOperationResult.Failure(
                "The command could not be completed.",
                exception.Message,
                "COMMAND_FAILED",
                exception);
        }
        finally
        {
            if (preventReentry)
            {
                runningCommands.TryRemove(command.Id, out _);
            }
        }
    }

    /// <summary>Returns whether a command with the supplied identifier is currently executing.</summary>
    public bool IsRunning(string commandId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        return runningCommands.ContainsKey(commandId);
    }
}
