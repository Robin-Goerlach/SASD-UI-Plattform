using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.WindowsSmokeChecks;

internal static class Program
{
    [STAThread]
    private static int Main()
    {
        string root = Path.Combine(Path.GetTempPath(), $"sasd-ui-windows-smoke-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);

        try
        {
            ValidateFileDrop(root);
            ValidateTrayLifetime();
            Console.WriteLine("SASD Windows integration smoke checks passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
        finally
        {
            try
            {
                Directory.Delete(root, recursive: true);
            }
            catch (IOException)
            {
                // Test cleanup should not hide a more useful assertion failure.
            }
            catch (UnauthorizedAccessException)
            {
                // A temporary-file scanner may briefly hold a file. CI cleanup can
                // remove the runner workspace even when this best-effort cleanup fails.
            }
        }
    }

    private static void ValidateFileDrop(string root)
    {
        string acceptedFile = Path.Combine(root, "accepted.txt");
        string rejectedFile = Path.Combine(root, "rejected.bin");
        string directory = Path.Combine(root, "folder");
        File.WriteAllText(acceptedFile, "small text payload");
        File.WriteAllBytes(rejectedFile, [1, 2, 3, 4]);
        Directory.CreateDirectory(directory);

        var data = new DataObject();
        data.SetData(DataFormats.FileDrop, new[] { acceptedFile, rejectedFile, directory });

        var policy = new SasdFileDropPolicy(
            AllowedExtensions: [".txt"],
            AllowDirectories: false,
            MaxItems: 5,
            MaxFileBytes: 1024);
        var service = new SasdDragDropService();
        SasdFileDropResult result = service.ReadFiles(data, policy);

        Ensure(result.Accepted.Count == 1, "File-drop validation accepted an unexpected number of items.");
        Ensure(result.Accepted[0].Path == Path.GetFullPath(acceptedFile), "Accepted file path was not normalized.");
        Ensure(result.Accepted[0].FileSizeBytes is > 0, "Accepted file metadata is missing.");
        Ensure(result.Rejected.Count == 2, "File-drop validation did not report both rejected items.");
        Ensure(result.Rejected.Any(item => item.ReasonCode == "DROP_EXTENSION_NOT_ALLOWED"), "Disallowed extension was not rejected.");
        Ensure(result.Rejected.Any(item => item.ReasonCode == "DROP_DIRECTORY_NOT_ALLOWED"), "Disallowed directory was not rejected.");
        Ensure(service.CanAccept(data, policy), "CanAccept disagrees with the validated drop result.");

        var duplicateData = new DataObject();
        duplicateData.SetData(DataFormats.FileDrop, new[] { acceptedFile, acceptedFile });
        SasdFileDropResult duplicateResult = service.ReadFiles(duplicateData, policy);
        Ensure(duplicateResult.Accepted.Count == 1, "Duplicate file path was accepted more than once.");
        Ensure(duplicateResult.Rejected.Single().ReasonCode == "DROP_DUPLICATE", "Duplicate path did not receive the expected reason code.");

        var tooManyData = new DataObject();
        tooManyData.SetData(DataFormats.FileDrop, new[] { acceptedFile, rejectedFile });
        SasdFileDropResult tooMany = service.ReadFiles(tooManyData, policy with { MaxItems = 1 });
        Ensure(!tooMany.HasAcceptedItems, "Over-sized file-drop payload was partially accepted.");
        Ensure(tooMany.Rejected.Single().ReasonCode == "DROP_TOO_MANY_ITEMS", "Over-sized payload was not rejected atomically.");

        var unsupported = new DataObject();
        unsupported.SetText("not a file drop");
        SasdFileDropResult unsupportedResult = service.ReadFiles(unsupported, policy);
        Ensure(!unsupportedResult.HasAcceptedItems, "Non-file drag payload was accepted.");
        Ensure(unsupportedResult.Rejected.Single().ReasonCode == "DROP_FORMAT_UNSUPPORTED", "Unsupported drag format received the wrong reason code.");
    }

    private static void ValidateTrayLifetime()
    {
        var tray = new SasdTrayService("SASD smoke");
        Ensure(!tray.IsVisible, "Tray service must start hidden.");
        Ensure(tray.Text == "SASD smoke", "Tray tooltip was not retained.");

        tray.Text = "SASD UI";
        tray.OpenText = "Show application";
        tray.ExitText = "Exit application";
        tray.SetIcon(SystemIcons.Information);
        Ensure(tray.Text == "SASD UI", "Tray tooltip update failed.");
        Ensure(tray.OpenText == "Show application" && tray.ExitText == "Exit application", "Tray menu labels did not update.");

        EnsureThrows<ArgumentOutOfRangeException>(
            () => tray.Text = new string('x', 64),
            "Tray service accepted tooltip text beyond the conservative Windows limit.");

        tray.Dispose();
        EnsureThrows<ObjectDisposedException>(
            () => _ = tray.Text,
            "Disposed tray service accepted property access.");
    }

    private static void EnsureThrows<TException>(Action action, string message)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
