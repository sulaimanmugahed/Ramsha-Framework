using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public class RamshaAsServiceModuleLoader
{
    public IModuleDescriptor[] LoadModules(
           IServiceCollection services,
           Type[] starterModuleTypes,
           ILogger logger)
    {
        var registry = new RamshaModuleRegistry();
        var allModules = new List<IRamshaModule>();

        logger.LogInformation("Registering {Count} starter modules", starterModuleTypes.Length);


        foreach (var starterType in starterModuleTypes)
        {
            var starterInstance = Activator.CreateInstance(starterType) as IRamshaModule
                ?? throw new InvalidOperationException($"Cannot create instance of {starterType.Name}");

            registry.RegisterModule(starterType, starterInstance);
            services.AddSingleton(starterType, starterInstance);
            allModules.Add(starterInstance);
        }


        foreach (var moduleInstance in allModules)
        {
            var registerContext = new RegisterContext(
                registry,
                moduleInstance.GetType(),
                services,
                logger);

            moduleInstance.Register(registerContext);
        }

        DiscoverAllModules(services, registry, logger);

        var descriptors = BuildDescriptorGraph(registry, logger);
        var sortedDescriptors = SortModulesByDependency(descriptors);

        logger.LogInformation("Loaded {TotalCount} modules from {StarterCount} starters",
            sortedDescriptors.Count, starterModuleTypes.Length);

        ModuleLogHelper.LogModuleDiagram(logger, sortedDescriptors);

        return sortedDescriptors.ToArray();
    }

    public IModuleDescriptor[] LoadModules(
        IServiceCollection services,
        IRamshaModule[] starterModuleInstances,
        ILogger logger)
    {
        var registry = new RamshaModuleRegistry();
        var starterTypes = starterModuleInstances.Select(m => m.GetType()).ToArray();

        logger.LogInformation("Registering {Count} starter modules", starterModuleInstances.Length);

        for (int i = 0; i < starterModuleInstances.Length; i++)
        {
            var instance = starterModuleInstances[i];
            var type = starterTypes[i];

            registry.RegisterModule(type, instance);
            services.AddSingleton(type, instance);
        }

        foreach (var moduleInstance in starterModuleInstances)
        {
            var registerContext = new RegisterContext(
                registry,
                moduleInstance.GetType(),
                services,
                logger);

            moduleInstance.Register(registerContext);
        }

        DiscoverAllModules(services, registry, logger);

        var descriptors = BuildDescriptorGraph(registry, logger);
        var sortedDescriptors = SortModulesByDependency(descriptors);

        logger.LogInformation("Loaded {TotalCount} modules from {StarterCount} starters",
            sortedDescriptors.Count, starterModuleInstances.Length);

        ModuleLogHelper.LogModuleDiagram(logger, sortedDescriptors);

        return sortedDescriptors.ToArray();
    }

    private void DiscoverAllModules(
        IServiceCollection services,
        IRamshaModuleRegistry registry,
        ILogger logger)
    {
        bool newModulesDiscovered;
        do
        {
            newModulesDiscovered = false;
            var currentModules = registry.GetAllModules().ToList();

            foreach (var moduleType in currentModules)
            {
                var instance = registry.GetInstance(moduleType);
                var context = new RegisterContext(registry, moduleType, services, logger);

                var beforeCount = registry.GetAllModules().Count();

                instance.Register(context);

                var afterCount = registry.GetAllModules().Count();
                if (afterCount > beforeCount)
                {
                    newModulesDiscovered = true;
                }
            }
        } while (newModulesDiscovered);
    }

    private Dictionary<Type, ModuleDescriptor> BuildDescriptorGraph(
        IRamshaModuleRegistry registry,
        ILogger logger)
    {
        var descriptors = new Dictionary<Type, ModuleDescriptor>();

        foreach (var moduleType in registry.GetAllModules())
        {
            var instance = registry.GetInstance(moduleType);
            descriptors[moduleType] = new ModuleDescriptor(moduleType, instance);
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
                    logger.LogWarning(
                        "Dependency {Dependency} of module {Module} not found in registry",
                        depType.Name, moduleType.Name);
                }
            }
        }

        return descriptors;
    }

    private List<IModuleDescriptor> SortModulesByDependency(
        Dictionary<Type, ModuleDescriptor> descriptors)
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