using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha.LocalMessaging.Abstractions;



public class LocalMessagingOptions
{
    private readonly List<Assembly> _assemblies = [];
    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    public LocalMessagingOptions AddMessagesFromAssembly(Assembly assembly)
    {
        if (!_assemblies.Any(x => x.FullName == assembly.FullName))
        {
            _assemblies.Add(assembly);
        }

        return this;
    }

    public LocalMessagingOptions AddMessagesFromAssembly<TAssemblyMarker>()
    {
        AddMessagesFromAssembly(typeof(TAssemblyMarker).Assembly);
        return this;
    }
}
