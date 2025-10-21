using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaAssemblyHelpers
{
    public static IEnumerable<Assembly> GetRamshaAssemblies()
    {
        var ramshaAssembly = typeof(IRamshaModule).Assembly;
        var ramshaName = ramshaAssembly.GetName().Name!;

        var allAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName))
            .ToList();

        var referencing = allAssemblies
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
