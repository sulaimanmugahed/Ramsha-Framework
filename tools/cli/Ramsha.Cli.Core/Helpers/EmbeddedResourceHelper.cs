using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class EmbeddedResourceHelper
{
    public static async Task<string> ReadAsStringAsync(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

        using (var stream = assembly.GetManifestResourceStream(fullResourceName))
        {
            if (stream == null)
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");

            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    public static Stream GetStream(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
        return assembly.GetManifestResourceStream(fullResourceName)
               ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
    }
}
