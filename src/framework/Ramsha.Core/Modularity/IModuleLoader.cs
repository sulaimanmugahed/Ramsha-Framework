using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ramsha;

public interface IModuleLoader
{
    IModuleDescriptor[] LoadModules(
         IServiceCollection services,
       Type startupModuleType,
       RegisterContext dependenciesContext
);
    IModuleDescriptor[] LoadModules(
          IServiceCollection services,
        IRamshaModule startupModuleInstance,
        RegisterContext dependenciesContext
 );
}
public class ModuleLoader : IModuleLoader
{

    public IModuleDescriptor[] LoadModules(IServiceCollection services, IRamshaModule startupInstance, RegisterContext moduleContext)
    {
        var startupModuleType = startupInstance.GetType();
        var visited = new HashSet<Type>();
        var moduleInstances = new Dictionary<Type, IRamshaModule>();
        var logger = services.GetBootstrapLogger<RamshaAppBase>();

        moduleInstances[startupModuleType] = startupInstance;
        moduleContext.SetCurrentModule(startupInstance);
        startupInstance.Register(moduleContext);


        var dependenciesTypes = moduleContext.GetDependenciesTypes(startupModuleType);

        foreach (var depType in dependenciesTypes)
        {
            AddModulesRecursively(services, depType, moduleContext, visited, moduleInstances, logger);
        }

        return GetDescriptors(moduleContext, startupModuleType, moduleInstances, logger);
    }
    public IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType, RegisterContext moduleContext)
    {
        var visited = new HashSet<Type>();
        var moduleInstances = new Dictionary<Type, IRamshaModule>();

        var logger = services.GetBootstrapLogger<RamshaAppBase>();

        AddModulesRecursively(services, startupModuleType, moduleContext, visited, moduleInstances, logger);

        return GetDescriptors(moduleContext, startupModuleType, moduleInstances, logger);
    }

    public IModuleDescriptor[] GetDescriptors(RegisterContext context, Type startupModuleType, Dictionary<Type, IRamshaModule> moduleInstances, ILogger<RamshaAppBase> logger)
    {
        var moduleDescriptors = new List<ModuleDescriptor>();
        var descriptorMap = new Dictionary<Type, ModuleDescriptor>();
        foreach (var moduleType in context.GetAllModulesTypes())
        {
            var descriptor = CreateModuleDescriptor(moduleType, moduleInstances[moduleType]);
            moduleDescriptors.Add(descriptor);
            descriptorMap[moduleType] = descriptor;
        }

        foreach (var descriptor in moduleDescriptors)
        {
            var dependencies = context.GetDependenciesTypes(descriptor.Type);
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

    private IRamshaModule CreateAndRegisterModuleInstance(Type moduleType, IServiceCollection services, ILogger<RamshaAppBase> logger)
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

        services.AddSingleton(moduleType, module);
        return module;
    }

    private void LoadDependenciesModules(
          IServiceCollection services,
         Type moduleType,
         RegisterContext context,
         Dictionary<Type, IRamshaModule> moduleInstances,
         ILogger<RamshaAppBase> logger)
    {
        var visited = new HashSet<Type>();

        var modulesTypes = context.GetDependenciesTypes(moduleType).ToList();

        foreach (var depType in modulesTypes)
        {
            AddModulesRecursively(services, depType, context, visited, moduleInstances, logger);
        }


    }

    // private void BuildModule(IRamshaModule module, IServiceCollection services, ModulesRegisterContext context)
    // {
    //     var builder = new ModuleBuilder(module.GetType(), services, context);
    //     module.OnLoading(builder);
    // }
    private void AddModulesRecursively(
         IServiceCollection services,
        Type moduleType,
        RegisterContext context,
        HashSet<Type> visited,
        Dictionary<Type, IRamshaModule> moduleInstances,
        ILogger<RamshaAppBase> logger)
    {
        if (!visited.Add(moduleType)) return;

        if (!moduleInstances.ContainsKey(moduleType))
        {
            var module = CreateAndRegisterModuleInstance(moduleType, services, logger);
            moduleInstances[moduleType] = module;
            context.SetCurrentModule(module);
            module.Register(context);
        }

        var modulesTypes = context.GetDependenciesTypes(moduleType).ToList();

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