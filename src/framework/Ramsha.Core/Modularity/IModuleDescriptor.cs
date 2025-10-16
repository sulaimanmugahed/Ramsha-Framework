using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha;

public interface IModuleDescriptor
{
    Type Type { get; }
    Assembly Assembly { get; }
    IRamshaModule Instance { get; }
    IReadOnlyList<IModuleDescriptor> Dependencies { get; }
}


public class ModuleDescriptor : IModuleDescriptor
{
    public ModuleDescriptor(Type moduleType, IRamshaModule instance)
    {
        Type = moduleType;
        Instance = instance;
        Assembly = moduleType.Assembly;
        _dependencies = [];
    }
    public void AddDependency(IModuleDescriptor descriptor)
    {
        if (!_dependencies.Any(x => x.Type == descriptor.Type))
        {
            _dependencies.Add(descriptor);
        }
    }
    public Type Type { get; }

    public Assembly Assembly { get; }

    public IRamshaModule Instance { get; }

    public IReadOnlyList<IModuleDescriptor> Dependencies => [.. _dependencies];
    private readonly List<IModuleDescriptor> _dependencies;
}
