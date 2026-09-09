using System.Xml.Linq;

namespace Sasd.Ui.ArchitectureChecks;

/// <summary>
/// Small dependency-free architecture test executable. Keeping these checks in
/// ordinary C# makes the architectural rules readable without requiring a
/// specialist test framework or a machine-specific Visual Studio extension.
/// </summary>
internal static class Program
{
    private const string KryptonPackage = "Krypton.Toolkit";
    private const string KryptonAdapterProject = "Sasd.Ui.WinForms.Krypton";

    private static int Main(string[] args)
    {
        try
        {
            string repositoryRoot = ResolveRepositoryRoot(args);
            string sourceRoot = Path.Combine(repositoryRoot, "src");
            Ensure(Directory.Exists(sourceRoot), $"Source directory was not found: {sourceRoot}");

            var projects = LoadProjects(sourceRoot);
            Ensure(projects.Count > 0, "No source projects were discovered.");

            ValidateCoreRemainsPlatformNeutral(projects);
            ValidateProductReferencesStayInsideSourceTree(sourceRoot, projects);
            ValidateKryptonIsolation(sourceRoot, projects);
            ValidateAcyclicProjectGraph(projects);

            Console.WriteLine($"SASD UI architecture checks passed for {projects.Count} source projects.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }

    private static string ResolveRepositoryRoot(string[] args)
    {
        string candidate = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
        return Path.GetFullPath(candidate);
    }

    private static Dictionary<string, ProjectInfo> LoadProjects(string sourceRoot)
    {
        var result = new Dictionary<string, ProjectInfo>(StringComparer.OrdinalIgnoreCase);

        foreach (string projectPath in Directory.EnumerateFiles(sourceRoot, "*.csproj", SearchOption.AllDirectories))
        {
            string fullPath = Path.GetFullPath(projectPath);
            var document = XDocument.Load(fullPath, LoadOptions.None);
            var project = new ProjectInfo(
                fullPath,
                Path.GetFileNameWithoutExtension(fullPath),
                ReadValues(document, "TargetFramework"),
                ReadValues(document, "TargetFrameworks"),
                ReadValues(document, "UseWindowsForms"),
                ReadIncludes(document, "ProjectReference"),
                ReadIncludes(document, "PackageReference"));

            result.Add(fullPath, project);
        }

        return result;
    }

    private static string[] ReadValues(XDocument document, string elementName) =>
        document.Descendants(elementName)
            .Select(element => element.Value.Trim())
            .Where(value => value.Length > 0)
            .ToArray();

    private static string[] ReadIncludes(XDocument document, string elementName) =>
        document.Descendants(elementName)
            .Select(element => element.Attribute("Include")?.Value?.Trim())
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!)
            .ToArray();

    private static void ValidateCoreRemainsPlatformNeutral(IReadOnlyDictionary<string, ProjectInfo> projects)
    {
        ProjectInfo core = projects.Values.Single(project => project.Name == "Sasd.Ui.Core");
        string[] targetFrameworks = core.TargetFrameworks
            .Concat(core.MultiTargetFrameworks.SelectMany(value => value.Split(';', StringSplitOptions.RemoveEmptyEntries)))
            .ToArray();

        Ensure(targetFrameworks.Length > 0, "Sasd.Ui.Core must declare a target framework.");
        Ensure(targetFrameworks.All(framework => !framework.Contains("-windows", StringComparison.OrdinalIgnoreCase)),
            "Sasd.Ui.Core must remain independent of a Windows-specific target framework.");
        Ensure(core.UseWindowsForms.All(value => !string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)),
            "Sasd.Ui.Core must not enable WinForms.");
        Ensure(core.ProjectReferences.Count == 0,
            "Sasd.Ui.Core must remain the lowest product layer and therefore cannot reference another SASD source project.");
    }

    private static void ValidateProductReferencesStayInsideSourceTree(
        string sourceRoot,
        IReadOnlyDictionary<string, ProjectInfo> projects)
    {
        string normalizedSourceRoot = EnsureTrailingSeparator(Path.GetFullPath(sourceRoot));

        foreach (ProjectInfo project in projects.Values)
        {
            string projectDirectory = Path.GetDirectoryName(project.Path)!;
            foreach (string reference in project.ProjectReferences)
            {
                string referencedPath = Path.GetFullPath(Path.Combine(projectDirectory, reference));
                Ensure(referencedPath.StartsWith(normalizedSourceRoot, StringComparison.OrdinalIgnoreCase),
                    $"Product project {project.Name} references a project outside src/: {reference}");
                Ensure(File.Exists(referencedPath),
                    $"Product project {project.Name} references a missing project: {reference}");
                Ensure(!string.Equals(project.Path, referencedPath, StringComparison.OrdinalIgnoreCase),
                    $"Project {project.Name} must not reference itself.");
            }
        }
    }

    private static void ValidateKryptonIsolation(
        string sourceRoot,
        IReadOnlyDictionary<string, ProjectInfo> projects)
    {
        foreach (ProjectInfo project in projects.Values)
        {
            bool hasKryptonPackage = project.PackageReferences.Any(package =>
                string.Equals(package, KryptonPackage, StringComparison.OrdinalIgnoreCase));

            if (hasKryptonPackage)
            {
                Ensure(project.Name == KryptonAdapterProject,
                    $"{KryptonPackage} may only be referenced by {KryptonAdapterProject}, not {project.Name}.");
            }
        }

        ProjectInfo adapter = projects.Values.Single(project => project.Name == KryptonAdapterProject);
        Ensure(adapter.PackageReferences.Any(package =>
                string.Equals(package, KryptonPackage, StringComparison.OrdinalIgnoreCase)),
            $"{KryptonAdapterProject} must explicitly reference the reviewed {KryptonPackage} package.");

        string adapterDirectory = EnsureTrailingSeparator(Path.GetDirectoryName(adapter.Path)!);
        foreach (string sourceFile in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories))
        {
            string fullPath = Path.GetFullPath(sourceFile);
            if (fullPath.StartsWith(adapterDirectory, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string text = File.ReadAllText(fullPath);
            Ensure(!text.Contains("Krypton.Toolkit", StringComparison.Ordinal) &&
                   !text.Contains("using Krypton.", StringComparison.Ordinal),
                $"Krypton implementation types leaked outside the adapter: {Path.GetRelativePath(sourceRoot, fullPath)}");
        }
    }

    private static void ValidateAcyclicProjectGraph(IReadOnlyDictionary<string, ProjectInfo> projects)
    {
        var visiting = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string projectPath in projects.Keys)
        {
            Visit(projectPath, projects, visiting, visited, new Stack<string>());
        }
    }

    private static void Visit(
        string projectPath,
        IReadOnlyDictionary<string, ProjectInfo> projects,
        ISet<string> visiting,
        ISet<string> visited,
        Stack<string> path)
    {
        if (visited.Contains(projectPath))
        {
            return;
        }

        if (!visiting.Add(projectPath))
        {
            string cycle = string.Join(" -> ", path.Reverse().Select(Path.GetFileNameWithoutExtension));
            throw new InvalidOperationException($"Cyclic source-project dependency detected: {cycle}");
        }

        path.Push(projectPath);
        ProjectInfo project = projects[projectPath];
        string projectDirectory = Path.GetDirectoryName(projectPath)!;

        foreach (string reference in project.ProjectReferences)
        {
            string target = Path.GetFullPath(Path.Combine(projectDirectory, reference));
            if (projects.ContainsKey(target))
            {
                Visit(target, projects, visiting, visited, path);
            }
        }

        path.Pop();
        visiting.Remove(projectPath);
        visited.Add(projectPath);
    }

    private static string EnsureTrailingSeparator(string path) =>
        path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private sealed record ProjectInfo(
        string Path,
        string Name,
        IReadOnlyList<string> TargetFrameworks,
        IReadOnlyList<string> MultiTargetFrameworks,
        IReadOnlyList<string> UseWindowsForms,
        IReadOnlyList<string> ProjectReferences,
        IReadOnlyList<string> PackageReferences);
}
