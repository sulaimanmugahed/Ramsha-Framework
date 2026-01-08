
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Ramsha;

public abstract class RamshaAppEngine : RamshaEngine, IRamshaAppEngine
{
    public Type StartupModuleType { get; }
    public IServiceCollection Services => _services;
    public string? ApplicationName { get; }
    public string InstanceId { get; }
    protected readonly AppCreationOptions _creationOptions;
    internal RamshaAppEngine(
         Type startupModuleType,
         IServiceCollection services,
         Action<AppCreationOptions>? optionsAction = null)
         : base(services)
    {
        StartupModuleType = startupModuleType;
        var options = new AppCreationOptions(services);
        optionsAction?.Invoke(options);
        _creationOptions = options;
        services.AddSingleton<IAppInfoAccessor>(this);
        InstanceId = Guid.NewGuid().ToString();
        ApplicationName = GetApplicationName(_creationOptions);
    }

    public virtual void Dispose()
    {
    }

    public override async Task ConfigureAsync()
    {
        await base.ConfigureAsync();
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
            var appNameConfig = configuration["AppName"];
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

    protected override IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services)
    {
        var loader = Services
          .GetSingletonInstance<IModuleLoader>();

        var logger = Services
            .GetBootstrapLogger<RamshaEngine>();

        return loader.LoadModules(
                Services,
                StartupModuleType,
                logger
            );
    }


    protected override void ConfigureCoreServices(IServiceCollection services)
    {
        base.ConfigureCoreServices(services);

        var moduleLoader = new RamshaModuleLoader();
        services.TryAddSingleton<IModuleLoader>(moduleLoader);

        services.AddSingleton<IRamshaHostEnvironment>(new RamshaHostEnvironment()
        {
            EnvironmentName = _creationOptions?.Environment
        });

        if (!services.IsAdded<IConfiguration>())
        {
            services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(
                    _creationOptions?.Configuration
                )
            );
        }
    }

}
