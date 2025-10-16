using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ramsha;

public interface IModuleManager
{
    Task InitializeModulesAsync(InitContext context);

    void InitializeModules(InitContext context);

    Task ShutdownModulesAsync(ShutdownContext context);

    void ShutdownModules(ShutdownContext context);
}
public class ModuleManager : IModuleManager
{
    private readonly IModuleContainer _moduleContainer;
    private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
    private readonly ILogger<ModuleManager> _logger;

    public ModuleManager(
        IModuleContainer moduleContainer,
     IOptions<ModuleLifecycleOptions> options,
     IServiceProvider serviceProvider,
     ILogger<ModuleManager> logger)
    {
        _logger = logger;
        _moduleContainer = moduleContainer;
        _lifecycleContributors = options.Value
           .Contributors
           .Select(serviceProvider.GetRequiredService)
           .Cast<IModuleLifecycleContributor>()
           .ToArray();

    }
    public void InitializeModules(InitContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    contributor.Init(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }

        _logger.LogInformation("All Modules Initialized Successfully.");
    }

    public async Task InitializeModulesAsync(InitContext context)
    {
        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in _moduleContainer.Modules)
            {
                try
                {
                    await contributor.InitAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }

        _logger.LogInformation("All Ramsha Modules Initialized Successfully.");

    }

    public void ShutdownModules(ShutdownContext context)
    {
        var modules = _moduleContainer.Modules.Reverse().ToList();

        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in modules)
            {
                try
                {
                    contributor.Shutdown(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }
    }

    public async Task ShutdownModulesAsync(ShutdownContext context)
    {
        var modules = _moduleContainer.Modules.Reverse().ToList();

        foreach (var contributor in _lifecycleContributors)
        {
            foreach (var module in modules)
            {
                try
                {
                    await contributor.ShutdownAsync(context, module.Instance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                }
            }
        }
    }
}
