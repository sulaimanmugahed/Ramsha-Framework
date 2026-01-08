using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public class RamshaModuleLoader : IModuleLoader
{
    public IModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType,
        ILogger logger
        )
    {
        var startupInstance = Activator.CreateInstance(startupModuleType) as IRamshaModule
            ?? throw new InvalidOperationException($"Cannot create instance of {startupModuleType.Name}");

        return LoadModules(services, startupInstance, logger);
    }

    public IModuleDescriptor[] LoadModules(
        IServiceCollection services,
        IRamshaModule startupModuleInstance,
        ILogger logger)
    {
        var registry = new RamshaModuleRegistry();

        var startupModuleType = startupModuleInstance.GetType();


        logger.LogInformation("Registering modules starting from {StartupModule}", startupModuleType.Name);

        registry.RegisterModule(startupModuleType, startupModuleInstance);
        services.AddSingleton(startupModuleType, startupModuleInstance);

        var registerContext = new RegisterContext(
            registry,
            startupModuleType,
            services,
            logger);

        startupModuleInstance.Register(registerContext);

        var descriptors = CreateDescriptors(registry, logger);

        var sortedDescriptors = SortDescriptor(descriptors);

        logger.LogInformation("Loaded {Count} modules", sortedDescriptors.Count);

        ModuleLogHelper.LogModuleDiagram(logger, sortedDescriptors);

        return sortedDescriptors.ToArray();
    }

    private Dictionary<Type, ModuleDescriptor> CreateDescriptors(IRamshaModuleRegistry registry, ILogger logger)
    {
        var descriptors = new Dictionary<Type, ModuleDescriptor>();

        foreach (var moduleType in registry.GetAllModules())
        {
            var instance = registry.GetInstance(moduleType);
            var descriptor = new ModuleDescriptor(moduleType, instance);
            descriptors[moduleType] = descriptor;
        }

        foreach (var moduleType in registry.GetAllModules())
        {
            var descriptor = descriptors[moduleType];
            var dependencies = registry.GetDependencies(moduleType);

            foreach (var depType in dependencies)
            {
                if (descriptors.TryGetValue(depType, out var depDescriptor))
                {
                    descriptor.AddDependency(depDescriptor);
                }
                else
                {
                    logger.LogWarning("Dependency {Dependency} of module {Module} not found in registry",
                        depType.Name, moduleType.Name);
                }
            }
        }

        return descriptors;
    }

    private List<IModuleDescriptor> SortDescriptor(Dictionary<Type, ModuleDescriptor> descriptors)
    {
        var visited = new HashSet<Type>();
        var tempMark = new HashSet<Type>();
        var result = new List<IModuleDescriptor>();
        var cycleDetected = false;
        Type? cycleStart = null;

        void Visit(Type moduleType)
        {
            if (tempMark.Contains(moduleType))
            {
                cycleDetected = true;
                cycleStart = moduleType;
                return;
            }

            if (visited.Contains(moduleType)) return;

            tempMark.Add(moduleType);

            if (descriptors.TryGetValue(moduleType, out var descriptor))
            {
                foreach (var dep in descriptor.Dependencies)
                {
                    Visit(dep.Type);
                    if (cycleDetected) return;
                }
            }

            tempMark.Remove(moduleType);
            visited.Add(moduleType);

            if (descriptors.TryGetValue(moduleType, out var desc))
            {
                result.Add(desc);
            }
        }

        foreach (var moduleType in descriptors.Keys)
        {
            if (!visited.Contains(moduleType))
            {
                Visit(moduleType);
                if (cycleDetected)
                {
                    throw new InvalidOperationException(
                        $"Circular dependency detected involving module {cycleStart?.Name} " +
                        $"Module dependency graph: {string.Join(" -> ", tempMark.Select(t => t.Name))}");
                }
            }
        }

        return result;
    }
}