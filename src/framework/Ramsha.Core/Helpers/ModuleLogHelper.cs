
using System.Text;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public static class ModuleLogHelper
{
    public static void LogModuleDiagram(
        this ILogger logger,
        IEnumerable<IModuleDescriptor> descriptors,
        LogLevel logLevel = LogLevel.Debug)
    {
        if (!logger.IsEnabled(logLevel)) return;

        var diagram = BuildModuleTreeDiagram(descriptors);
        logger.Log(logLevel, diagram);
    }

    public static string BuildModuleTreeDiagram(IEnumerable<IModuleDescriptor> descriptors)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("╔════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                    MODULE DEPENDENCY TREE                      ║");
        sb.AppendLine("╚════════════════════════════════════════════════════════════════╝");
        sb.AppendLine();

        var descriptorList = descriptors.ToList();
        if (!descriptorList.Any())
        {
            sb.AppendLine("  (No modules found)");
            return sb.ToString();
        }

        var moduleLookup = descriptorList.ToDictionary(d => d.Type);
        var visited = new HashSet<Type>();

        // Find root modules - modules that NO other module depends on
        var allDependencyTypes = descriptorList
            .SelectMany(d => d.Dependencies.Select(dep => dep.Type))
            .ToHashSet();

        var roots = descriptorList
            .Where(d => !allDependencyTypes.Contains(d.Type))
            .OrderByDescending(d => d.Dependencies.Count())  // Prefer modules with more dependencies as primary root
            .ToList();

        // If no roots found (pure circular dependencies), pick the module with most dependencies
        if (!roots.Any())
        {
            roots = new List<IModuleDescriptor>
            {
                descriptorList.OrderByDescending(d => d.Dependencies.Count()).First()
            };
        }

        // Build tree for each root
        for (int i = 0; i < roots.Count; i++)
        {
            if (i > 0)
            {
                sb.AppendLine();
                sb.AppendLine("─────────────────────────────────────────────────────────────────");
                sb.AppendLine();
            }
            BuildTopDownTree(sb, roots[i], moduleLookup, visited, 0, new List<bool>(), new HashSet<Type>());
        }

        // Handle any unvisited modules
        var unvisited = descriptorList.Where(d => !visited.Contains(d.Type)).ToList();
        if (unvisited.Any())
        {
            sb.AppendLine();
            sb.AppendLine("─────────────────────────────────────────────────────────────────");
            sb.AppendLine("📦 Additional Modules:");
            sb.AppendLine();
            foreach (var module in unvisited)
            {
                sb.AppendLine($"  • {module.Type.Name}");
                if (module.Dependencies.Any())
                {
                    sb.AppendLine($"    └─ Depends on: {string.Join(", ", module.Dependencies.Select(d => d.Type.Name))}");
                }
            }
        }

        return sb.ToString();
    }

    private static void BuildTopDownTree(
        StringBuilder sb,
        IModuleDescriptor module,
        Dictionary<Type, IModuleDescriptor> moduleLookup,
        HashSet<Type> visited,
        int level,
        List<bool> isLastAtLevel,
        HashSet<Type> currentPath)
    {
        // Detect circular reference in current path
        if (currentPath.Contains(module.Type))
        {
            AppendModuleLine(sb, $"{module.Type.Name} ↻ (circular)", level, isLastAtLevel, false);
            return;
        }

        // Mark as visited
        if (visited.Contains(module.Type))
        {
            AppendModuleLine(sb, $"{module.Type.Name} (see above)", level, isLastAtLevel, false);
            return;
        }

        visited.Add(module.Type);
        currentPath.Add(module.Type);

        // Display the current module
        bool isRoot = level == 0;
        AppendModuleLine(sb, module.Type.Name, level, isLastAtLevel, isRoot);

        // Get dependencies (modules this one depends on)
        var dependencies = module.Dependencies.ToList();

        if (dependencies.Any())
        {
            // Add connector line showing downward flow
            if (!isRoot)
            {
                string indent = GetIndent(level, isLastAtLevel);
                sb.AppendLine($"{indent}     │");
            }
            else
            {
                sb.AppendLine("       │");
            }

            for (int i = 0; i < dependencies.Count; i++)
            {
                bool isLast = i == dependencies.Count - 1;
                var depType = dependencies[i].Type;

                // Update the isLastAtLevel list for the next level
                var newIsLastAtLevel = new List<bool>(isLastAtLevel);
                if (level > 0 || dependencies.Count > 1)
                {
                    newIsLastAtLevel.Add(isLast);
                }

                // Show arrow pointing down to dependency
                string indent = GetIndent(level, isLastAtLevel);
                string connector = isLast ? "└" : "├";

                if (!isRoot && dependencies.Count > 1)
                {
                    sb.AppendLine($"{indent}     {connector}─────▼");
                }
                else if (isRoot && dependencies.Count > 1)
                {
                    sb.AppendLine($"       {connector}─────▼");
                }
                else
                {
                    if (!isRoot)
                    {
                        sb.AppendLine($"{indent}     └─────▼");
                    }
                    else
                    {
                        sb.AppendLine("       └─────▼");
                    }
                }

                // Recursively build the dependency's subtree
                if (moduleLookup.TryGetValue(depType, out var dependencyModule))
                {
                    var newPath = new HashSet<Type>(currentPath);
                    BuildTopDownTree(sb, dependencyModule, moduleLookup, visited, level + 1, newIsLastAtLevel, newPath);
                }
                else
                {
                    // External dependency not in our module list
                    AppendModuleLine(sb, $"{depType.Name} (external)", level + 1, newIsLastAtLevel, false);
                }

                // Add spacing between siblings
                if (!isLast && i < dependencies.Count - 1)
                {
                    string spacing = GetIndent(level, isLastAtLevel);
                    sb.AppendLine($"{spacing}     │");
                }
            }
        }

        currentPath.Remove(module.Type);
    }

    private static void AppendModuleLine(
        StringBuilder sb,
        string moduleName,
        int level,
        List<bool> isLastAtLevel,
        bool isRoot)
    {
        if (isRoot)
        {
            // Root module - centered with a box
            string boxedName = $"┌─────────────────────┐";
            string nameLine = $"│  {moduleName.PadRight(17)}  │";
            string bottomLine = $"└─────────────────────┘";

            int padding = 3;
            sb.AppendLine($"{new string(' ', padding)}{boxedName}");
            sb.AppendLine($"{new string(' ', padding)}{nameLine}");
            sb.AppendLine($"{new string(' ', padding)}{bottomLine}");
        }
        else
        {
            string indent = GetIndent(level, isLastAtLevel);
            sb.AppendLine($"{indent}     [{moduleName}]");
        }
    }

    private static string GetIndent(int level, List<bool> isLastAtLevel)
    {
        if (level == 0) return "";

        var sb = new StringBuilder();

        // Start with base indentation
        sb.Append("       ");

        // Add tree structure characters based on ancestor positions
        for (int i = 0; i < isLastAtLevel.Count; i++)
        {
            if (isLastAtLevel[i])
            {
                sb.Append("      ");
            }
            else
            {
                sb.Append("│     ");
            }
        }

        return sb.ToString();
    }



    private static void CalculateDepths(
        IModuleDescriptor module,
        Dictionary<Type, IModuleDescriptor> moduleLookup,
        Dictionary<Type, int> depthMap,
        HashSet<Type> visited,
        int currentDepth,
        HashSet<Type> currentPath)
    {
        // Prevent infinite recursion
        if (currentPath.Contains(module.Type))
        {
            return;
        }

        // Update depth (use maximum depth if module is reachable via multiple paths)
        if (!depthMap.ContainsKey(module.Type) || depthMap[module.Type] < currentDepth)
        {
            depthMap[module.Type] = currentDepth;
        }

        if (visited.Contains(module.Type))
        {
            return;
        }

        visited.Add(module.Type);
        currentPath.Add(module.Type);

        // Process dependencies
        foreach (var dep in module.Dependencies)
        {
            if (moduleLookup.TryGetValue(dep.Type, out var depModule))
            {
                var newPath = new HashSet<Type>(currentPath);
                CalculateDepths(depModule, moduleLookup, depthMap, visited, currentDepth + 1, newPath);
            }
        }

        currentPath.Remove(module.Type);
    }

}
