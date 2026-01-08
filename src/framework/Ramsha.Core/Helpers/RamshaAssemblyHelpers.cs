using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace Ramsha;

public class RamshaAssemblyHelpers
{
    private static bool _loaded = false;
    private static List<Assembly> _cachedAssemblies = [];
    private static readonly object _lock = new();

    public static void LoadSolutionDlls(string[]? exclude = null)
    {
        lock (_lock)
        {
            if (_loaded) return;

            exclude ??= ["Microsoft.", "System.", "AspNetCore"];

            _cachedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName) && !exclude.Any(a.FullName.StartsWith))
            .ToList();

            _loaded = true;
        }
    }


    public static IEnumerable<Assembly> GetAssembliesWithAccessTo(Type type)
    {
        var target = type.Assembly.GetName().Name!;
        var result = new HashSet<Assembly>();

        foreach (var asm in _cachedAssemblies)
        {
            if (HasReferenceRecursive(asm, target, new HashSet<string>()))
                result.Add(asm);
        }

        result.Add(type.Assembly);
        return result;
    }

    private static bool HasReferenceRecursive(Assembly asm, string target, HashSet<string> visited)
    {
        if (!visited.Add(asm.GetName().Name!))
            return false;

        try
        {
            foreach (var r in asm.GetReferencedAssemblies())
            {
                if (string.Equals(r.Name, target, StringComparison.OrdinalIgnoreCase))
                    return true;

                var loaded = _cachedAssemblies
                    .FirstOrDefault(a => string.Equals(a.GetName().Name, r.Name, StringComparison.OrdinalIgnoreCase));

                if (loaded != null && HasReferenceRecursive(loaded, target, visited))
                    return true;
            }
        }
        catch { }

        return false;
    }

    public static IEnumerable<Assembly> GetAssembliesWithAccessToRamsha()
    {
        return GetAssembliesWithAccessTo(typeof(IRamshaModule));
    }

    public static IEnumerable<Assembly> GetAssembliesWithDirectReferenceTo(Type referenceMarker)
    {
        var ramshaAssembly = referenceMarker.Assembly;
        var ramshaName = ramshaAssembly.GetName().Name!;

        var referencing = _cachedAssemblies
            .Where(a =>
            {
                try
                {
                    return a.GetReferencedAssemblies()
                            .Any(r => string.Equals(r.Name, ramshaName, StringComparison.OrdinalIgnoreCase));
                }
                catch
                {
                    return false;
                }
            })
            .ToList();

        referencing.Add(ramshaAssembly);

        return referencing.Distinct().ToList();
    }

}
