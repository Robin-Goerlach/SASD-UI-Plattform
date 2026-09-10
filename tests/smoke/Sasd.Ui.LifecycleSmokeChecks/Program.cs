using Sasd.Ui.WinForms.Media;
using Sasd.Ui.WinForms.Shell;
using Sasd.Ui.WinForms.Windows;

namespace Sasd.Ui.LifecycleSmokeChecks;

internal static class Program
{
    // This is deliberately an endurance smoke check, not a benchmark. A moderate fixed
    // cycle count is enough to expose obvious lifecycle mistakes while keeping CI fast and
    // independent of machine-specific handle-count or timing thresholds.
    private const int CycleCount = 64;

    [STAThread]
    private static int Main()
    {
        try
        {
            ValidateImageViewerLifecycle();
            ValidateDocumentTabsLifecycle();
            ValidateTrayServiceLifecycle();

            Console.WriteLine($"SASD lifecycle smoke checks passed ({CycleCount} cycles per scenario).");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ValidateImageViewerLifecycle()
    {
        for (int cycle = 0; cycle < CycleCount; cycle++)
        {
            var viewer = new SasdImageViewer
            {
                Size = new Size(320, 200),
            };

            int imageChangedCount = 0;
            int zoomChangedCount = 0;
            EventHandler imageChanged = (_, _) => imageChangedCount++;
            EventHandler zoomChanged = (_, _) => zoomChangedCount++;
            viewer.ImageChanged += imageChanged;
            viewer.ZoomChanged += zoomChanged;

            using (var source = new Bitmap(48 + cycle % 8, 36 + cycle % 6))
            {
                source.SetPixel(0, 0, Color.Black);
                viewer.SetImage(source);

                Ensure(viewer.HasImage, "Image viewer did not retain its owned image clone.");
                Ensure(viewer.Image is not null, "Image viewer reported HasImage without an image instance.");
                Ensure(!ReferenceEquals(source, viewer.Image), "Image viewer retained the caller-owned image instead of cloning it.");
            }

            // The caller-owned bitmap has already been disposed. Reading the viewer copy after
            // that point exercises the documented ownership boundary rather than only testing a
            // same-scope assignment.
            Ensure(viewer.Image is { Width: > 0, Height: > 0 }, "Viewer-owned image did not survive disposal of the caller source.");

            _ = viewer.Handle;
            viewer.ZoomMode = SasdImageZoomMode.ActualSize;
            viewer.ZoomIn(25);
            viewer.ZoomOut(15);
            viewer.ResetZoom();
            viewer.ClearImage();

            Ensure(!viewer.HasImage, "ClearImage left an owned image behind.");
            Ensure(imageChangedCount == 2, "ImageChanged did not describe exactly the assign/clear lifecycle.");
            Ensure(zoomChangedCount > 0, "Zoom lifecycle did not raise any observable change notification.");

            viewer.ImageChanged -= imageChanged;
            viewer.ZoomChanged -= zoomChanged;
            viewer.Dispose();
            viewer.Dispose();

            Ensure(!viewer.HasImage, "Disposing the image viewer retained owned image state.");
        }
    }

    private static void ValidateDocumentTabsLifecycle()
    {
        for (int cycle = 0; cycle < CycleCount; cycle++)
        {
            var host = new SasdDocumentTabs
            {
                Size = new Size(640, 360),
            };

            int openedCount = 0;
            int closedCount = 0;
            int selectedCount = 0;
            int closedWithDisposedContent = 0;
            var contents = new List<Label>();

            EventHandler<SasdDocumentEventArgs> opened = (_, _) => openedCount++;
            EventHandler<SasdDocumentEventArgs> selected = (_, _) => selectedCount++;
            EventHandler<SasdDocumentEventArgs> closed = (_, e) =>
            {
                closedCount++;
                if (e.Content.IsDisposed)
                {
                    closedWithDisposedContent++;
                }
            };

            host.DocumentOpened += opened;
            host.DocumentSelected += selected;
            host.DocumentClosed += closed;
            _ = host.Handle;

            for (int document = 0; document < 3; document++)
            {
                string documentId = $"cycle-{cycle}-document-{document}";
                Control content = host.OpenOrSelect(
                    documentId,
                    $"Document {document + 1}",
                    () =>
                    {
                        var label = new Label
                        {
                            AutoSize = true,
                            Text = $"Lifecycle content {cycle}/{document}",
                        };
                        contents.Add(label);
                        return label;
                    });

                Ensure(ReferenceEquals(content, contents[^1]), "Document host did not retain the factory-created content instance.");
            }

            Ensure(host.DocumentCount == 3, "Document host did not track all opened documents.");
            Ensure(host.SelectDocument($"cycle-{cycle}-document-0"), "Existing document could not be selected.");
            Ensure(host.SelectedDocumentId == $"cycle-{cycle}-document-0", "Selected document id did not follow selection.");
            Ensure(host.SetDocumentTitle($"cycle-{cycle}-document-0", "Renamed document"), "Existing document title could not be changed.");

            Ensure(host.CloseDocument($"cycle-{cycle}-document-1"), "Existing document could not be closed.");
            Ensure(contents[1].IsDisposed, "Closing a document did not dispose host-owned content.");
            Ensure(host.DocumentCount == 2, "Document count did not shrink after close.");

            host.CloseAllDocuments();
            Ensure(host.DocumentCount == 0, "CloseAllDocuments left documents registered.");
            Ensure(contents.All(content => content.IsDisposed), "CloseAllDocuments did not dispose every host-owned content control.");
            Ensure(openedCount == 3, "DocumentOpened count did not match created documents.");
            Ensure(closedCount == 3, "DocumentClosed count did not match disposed documents.");
            Ensure(closedWithDisposedContent == 3, "DocumentClosed was raised before owned content had been disposed.");
            Ensure(selectedCount > 0, "Document selection produced no observable selection event.");

            host.DocumentOpened -= opened;
            host.DocumentSelected -= selected;
            host.DocumentClosed -= closed;
            host.Dispose();
            host.Dispose();

            Ensure(host.DocumentCount == 0, "Disposing an empty document host recreated or retained document state.");
        }
    }

    private static void ValidateTrayServiceLifecycle()
    {
        for (int cycle = 0; cycle < CycleCount; cycle++)
        {
            var service = new SasdTrayService(SystemIcons.Application, $"SASD lifecycle {cycle}");

            // Endurance verification must not flash dozens of notification-area icons on the CI
            // desktop. The service's safer default is therefore exercised without calling Show().
            Ensure(!service.IsVisible, "Tray service became visible during construction.");

            service.Text = $"SASD cycle {cycle}";
            service.OpenText = "Open test application";
            service.ExitText = "Exit test application";
            service.SetIcon(SystemIcons.Information);

            service.Dispose();
            service.Dispose();

            Ensure(!service.IsVisible, "Disposed tray service still reports a visible icon.");
            ExpectObjectDisposed(() => _ = service.Text, "Disposed tray service allowed Text access.");
            ExpectObjectDisposed(service.Show, "Disposed tray service allowed Show().");
        }
    }

    private static void ExpectObjectDisposed(Action action, string message)
    {
        try
        {
            action();
        }
        catch (ObjectDisposedException)
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
