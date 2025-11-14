using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public abstract class RamshaAppBase : IRamshaApp
{
    public Type StartupModuleType { get; }

    public IServiceProvider ServiceProvider { get; private set; } = default!;

    public IServiceCollection Services { get; }

    public IReadOnlyList<IModuleDescriptor> Modules { get; }

    public string? ApplicationName { get; }

    public string InstanceId { get; } = Guid.NewGuid().ToString();

    private bool _configuredServices;

    internal RamshaAppBase(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction,
        IRamshaModule? startupModuleInstance = null)
    {
        StartupModuleType = startupModuleType;
        Services = services;

        services.TryAddObjectAccessor<IServiceProvider>();

        var options = new AppCreationOptions(services);
        optionsAction?.Invoke(options);

        ApplicationName = GetApplicationName(options);

        services.AddSingleton<IRamshaApp>(this);
        services.AddSingleton<IAppInfoAccessor>(this);
        services.AddSingleton<IModuleContainer>(this);
        services.AddSingleton<IRamshaHostEnvironment>(new RamshaHostEnvironment()
        {
            EnvironmentName = options.Environment
        });


        services.AddCoreServices(this, options);

        var dependenciesContext = new AppModulesContext();
        services.AddSingleton(dependenciesContext);

        Modules = LoadModules(services, options, dependenciesContext, startupModuleInstance);

        if (!options.SkipConfigureServices)
        {
            ConfigureServices();
        }
    }

    public virtual async Task ShutdownAsync()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .ShutdownModulesAsync(new ShutdownContext(scope.ServiceProvider));
        }
    }

    public virtual void Shutdown()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .ShutdownModules(new ShutdownContext(scope.ServiceProvider));
        }
    }

    public virtual void Dispose()
    {
    }

    protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
    }

    protected virtual async Task InitializeModulesAsync()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            WriteInitLogs(scope.ServiceProvider);
            await scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModulesAsync(new InitContext(scope.ServiceProvider));
        }
    }

    protected virtual void InitializeModules()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            WriteInitLogs(scope.ServiceProvider);
            scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModules(new InitContext(scope.ServiceProvider));
        }
    }

    protected virtual void WriteInitLogs(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<RamshaAppBase>>();
        if (logger == null)
        {
            return;
        }

        var bootstrapLogger = serviceProvider.GetRequiredService<IBootstrapLoggerFactory>().Create<RamshaAppBase>();
        bootstrapLogger.ReplayTo(logger);
        bootstrapLogger.ClearEntries();
    }



    protected virtual IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services, AppCreationOptions options, AppModulesContext dependenciesContext, IRamshaModule? startupModuleInstance = null)
    {
        var loader = services
            .GetSingletonInstance<IModuleLoader>();

        return startupModuleInstance is null
        ? loader.LoadModules(
                services,
                StartupModuleType,
                dependenciesContext
            )
        : loader.LoadModules(services,
                startupModuleInstance,
                dependenciesContext);

    }


    public virtual async Task ConfigureServicesAsync()
    {
        CheckMultipleConfigureServices();

        var context = new ConfigureContext(Services);
        Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is RamshaModule ramshaModule)
            {
                ramshaModule.ConfigureContext = context;
            }
        }

        foreach (var module in Modules)
        {
            try
            {
                await module.Instance.OnConfiguringAsync(context);
            }

            catch (Exception ex)
            {
                throw new Exception($"An error occurred during {nameof(IRamshaModule.OnConfiguringAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        _configuredServices = true;
        TryToSetEnvironment(Services);
    }

    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new Exception("Services have already been configured! If you call ConfigureServicesAsync method, you must have set AbpApplicationCreationOptions.SkipConfigureServices to true before.");
        }
    }

    public virtual void ConfigureServices()
    {
        CheckMultipleConfigureServices();

        var context = new ConfigureContext(Services);
        Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is RamshaModule abpModule)
            {
                abpModule.ConfigureContext = context;
            }
        }


        foreach (var module in Modules)
        {
            try
            {
                module.Instance.OnConfiguring(context);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during {nameof(IRamshaModule.OnConfiguring)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        _configuredServices = true;
        TryToSetEnvironment(Services);
    }

    private static string? GetApplicationName(AppCreationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.AppName))
        {
            return options.AppName!;
        }

        var configuration = options.Services.GetConfigurationOrNull();
        if (configuration != null)
        {
            var appNameConfig = configuration["ApplicationName"];
            if (!string.IsNullOrWhiteSpace(appNameConfig))
            {
                return appNameConfig!;
            }
        }

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            return entryAssembly.GetName().Name;
        }

        return null;
    }

    private static void TryToSetEnvironment(IServiceCollection services)
    {
        var abpHostEnvironment = services.GetSingletonInstance<IRamshaHostEnvironment>();
        if (string.IsNullOrWhiteSpace(abpHostEnvironment.EnvironmentName))
        {
            abpHostEnvironment.EnvironmentName = Environments.Production;
        }
    }
}