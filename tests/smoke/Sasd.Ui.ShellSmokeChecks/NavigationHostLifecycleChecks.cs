using Sasd.Ui.WinForms.Shell;

namespace Sasd.Ui.ShellSmokeChecks;

/// <summary>
/// Focused ownership checks for runtime navigation-cache transitions. These scenarios exercise
/// public behavior only: whether factory-created views are reused or disposed when the existing
/// CachePages policy changes during a long-running application session.
/// </summary>
internal static class NavigationHostLifecycleChecks
{
    public static void Run()
    {
        ValidateAccessibilitySurface();
        ValidateCachePolicyTransitions();
        ValidateDisposedCachedViewRecovery();
    }

    private static void ValidateAccessibilitySurface()
    {
        using var host = new SasdNavigationHost();

        Ensure(host.AutoScaleMode == AutoScaleMode.Dpi,
            "Navigation host is not configured for DPI-aware scaling.");
        Ensure(host.AccessibleRole == AccessibleRole.Grouping && host.AccessibleName == "Navigation",
            "Navigation host does not expose stable group-level accessibility semantics.");

        SplitContainer split = host.Controls.OfType<SplitContainer>().Single();
        ListBox navigationList = split.Panel1.Controls.OfType<ListBox>().Single();
        Panel contentPanel = split.Panel2.Controls.OfType<Panel>().Single();

        Ensure(navigationList.AccessibleName == "Navigation pages" &&
               !string.IsNullOrWhiteSpace(navigationList.AccessibleDescription),
            "Navigation page list does not expose a descriptive accessible contract.");
        Ensure(contentPanel.AccessibleRole == AccessibleRole.Pane &&
               contentPanel.AccessibleName == "Page content" &&
               !contentPanel.TabStop,
            "Navigation content area does not expose non-interactive pane semantics.");
    }

    private static void ValidateCachePolicyTransitions()
    {
        var host = new SasdNavigationHost { CachePages = true };
        TrackingPanel? firstA = null;
        TrackingPanel? firstB = null;
        TrackingPanel? secondA = null;
        TrackingPanel? secondB = null;
        int aCreations = 0;
        int bCreations = 0;

        host.RegisterPage("a", "Page A", () =>
        {
            var view = new TrackingPanel($"A{++aCreations}");
            if (aCreations == 1)
            {
                firstA = view;
            }
            else
            {
                secondA = view;
            }

            return view;
        });
        host.RegisterPage("b", "Page B", () =>
        {
            var view = new TrackingPanel($"B{++bCreations}");
            if (bCreations == 1)
            {
                firstB = view;
            }
            else
            {
                secondB = view;
            }

            return view;
        });

        try
        {
            Ensure(host.Navigate("a"),
                "Navigation host could not navigate to the first cached Page A view.");
            TrackingPanel observedFirstA = firstA
                ?? throw new InvalidOperationException("Page A factory did not provide its first view.");

            Ensure(host.Navigate("b"),
                "Navigation host could not navigate to the first cached Page B view.");
            TrackingPanel observedFirstB = firstB
                ?? throw new InvalidOperationException("Page B factory did not provide its first view.");
            Ensure(!observedFirstA.IsDisposed,
                "Cached inactive Page A was disposed while caching remained enabled.");

            // true -> false is an ownership transition. Inactive cached views must be released
            // immediately, while the currently displayed view remains alive until navigation
            // actually leaves it.
            host.CachePages = false;
            Ensure(observedFirstA.IsDisposed,
                "Disabling page caching retained an inactive cached view.");
            Ensure(!observedFirstB.IsDisposed,
                "Disabling page caching disposed the currently displayed view prematurely.");

            Ensure(host.Navigate("a"),
                "Navigation with caching disabled could not return to Page A.");
            TrackingPanel observedSecondA = secondA
                ?? throw new InvalidOperationException("Page A factory did not create its replacement view.");
            Ensure(observedFirstB.IsDisposed,
                "Leaving the current page with caching disabled did not dispose that view.");
            Ensure(aCreations == 2,
                "Page A factory count does not match the expected cache-disabled recreation.");

            // false -> true must adopt the currently displayed, already host-owned view. Without
            // that adoption, the next navigation could detach it while skipping both disposal
            // and cache retention because the policy had just changed to true.
            host.CachePages = true;
            Ensure(host.Navigate("b"),
                "Re-enabling caching could not navigate to Page B.");
            TrackingPanel observedSecondB = secondB
                ?? throw new InvalidOperationException("Page B factory did not create its replacement view.");
            Ensure(!observedSecondA.IsDisposed,
                "Current Page A was lost instead of being adopted when caching was enabled.");

            Ensure(host.Navigate("a"),
                "Navigation host could not return to the adopted Page A view.");
            Ensure(aCreations == 2 && !observedSecondA.IsDisposed,
                "Revisiting Page A did not reuse the view adopted during false-to-true cache transition.");
            Ensure(!observedSecondB.IsDisposed,
                "Inactive Page B was disposed even though caching was enabled.");
        }
        finally
        {
            host.Dispose();
        }

        Ensure(secondA is not null && secondA.IsDisposed,
            "Disposing the navigation host did not dispose its current owned view.");
        Ensure(secondB is not null && secondB.IsDisposed,
            "Disposing the navigation host did not dispose its inactive cached view.");
    }

    private static void ValidateDisposedCachedViewRecovery()
    {
        using var host = new SasdNavigationHost { CachePages = true };
        TrackingPanel? first = null;
        TrackingPanel? replacement = null;
        int creations = 0;

        host.RegisterPage("a", "Page A", () =>
        {
            var view = new TrackingPanel($"A{++creations}");
            if (creations == 1)
            {
                first = view;
            }
            else
            {
                replacement = view;
            }

            return view;
        });
        host.RegisterPage("b", "Page B", static () => new Panel());

        Ensure(host.Navigate("a"),
            "Disposed-cache recovery setup could not navigate to Page A.");
        TrackingPanel observedFirst = first
            ?? throw new InvalidOperationException("Disposed-cache recovery Page A factory did not return a view.");
        Ensure(host.Navigate("b"),
            "Disposed-cache recovery setup did not move Page A into the inactive cache.");

        // Factory-created views belong to the host, so application code should not dispose a
        // cached view. Recovering from that misuse is nevertheless safer than attempting to
        // reattach a disposed WinForms control to the live content tree.
        observedFirst.Dispose();
        Ensure(host.Navigate("a"),
            "Navigation host did not recover from an unexpectedly disposed cached view.");
        TrackingPanel observedReplacement = replacement
            ?? throw new InvalidOperationException("Disposed cached view was not replaced by the page factory.");
        Ensure(creations == 2 && !observedReplacement.IsDisposed,
            "Disposed cached view was reused instead of being replaced by the page factory.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed class TrackingPanel(string instanceName) : Panel
    {
        public string InstanceName { get; } = instanceName;
    }
}
