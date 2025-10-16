using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ramsha;

public interface IModuleLoader
{
    IModuleDescriptor[] LoadModules(
         IServiceCollection services,
       Type startupModuleType,
       AppModulesContext dependenciesContext
    );
}
public class ModuleLoader : IModuleLoader
{
    public IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType, AppModulesContext dependenciesContext)
    {
        var visited = new HashSet<Type>();
        var moduleInstances = new Dictionary<Type, IRamshaModule>();

        var logger = services.GetBootstrapLogger<RamshaAppBase>();


        AddModulesRecursively(services, startupModuleType, dependenciesContext, visited, moduleInstances, logger);


        var moduleDescriptors = new List<ModuleDescriptor>();
        var descriptorMap = new Dictionary<Type, ModuleDescriptor>();
        foreach (var moduleType in dependenciesContext.GetAllModulesTypes())
        {
            var descriptor = CreateModuleDescriptor(moduleType, moduleInstances[moduleType]);
            moduleDescriptors.Add(descriptor);
            descriptorMap[moduleType] = descriptor;
        }

        foreach (var descriptor in moduleDescriptors)
        {
            var dependencies = dependenciesContext.GetDependenciesTypes(descriptor.Type);
            foreach (var depType in dependencies)
            {
                if (descriptorMap.TryGetValue(depType, out var depDescriptor))
                {
                    descriptor.AddDependency(depDescriptor);
                }
            }
            logger.LogInformation("Module {Module} Loaded .", descriptor.Type.FullName);
        }

        moduleDescriptors.Reverse();
        moduleDescriptors.MoveItem(x => x.Type == startupModuleType, moduleDescriptors.Count - 1);

        return moduleDescriptors.Cast<IModuleDescriptor>().ToArray();
    }
    private void AddModulesRecursively(
         IServiceCollection services,
        Type moduleType,
        AppModulesContext context,
        HashSet<Type> visited,
        Dictionary<Type, IRamshaModule> moduleInstances,
        ILogger<RamshaAppBase> logger)
    {
        if (!visited.Add(moduleType)) return;

        if (!moduleInstances.ContainsKey(moduleType))
        {
            IRamshaModule module;
            try
            {
                module = (IRamshaModule)Activator.CreateInstance(moduleType)!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create module instance: {ModuleType}", moduleType.FullName);
                throw;
            }
            moduleInstances[moduleType] = module;
            services.AddSingleton(moduleType, module);

            var builder = new ModuleBuilder(moduleType, services, context);
            module.OnModuleCreating(builder);
        }

        var modulesTypes = context.GetAllModulesTypes().ToList();

        foreach (var depType in modulesTypes)
        {
            if (!visited.Contains(depType))
                AddModulesRecursively(services, depType, context, visited, moduleInstances, logger);
        }


    }


    private ModuleDescriptor CreateModuleDescriptor(Type moduleType, IRamshaModule moduleInstance)
    {
        return new ModuleDescriptor(moduleType, moduleInstance);
    }
}