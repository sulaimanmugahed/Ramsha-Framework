using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ramsha;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddCoreServices(this IServiceCollection services,
        IRamshaApp application,
        AppCreationOptions applicationCreationOptions)
    {
        services.AddOptions();
        services.AddLogging();
        services.AddLocalization();

        var moduleLoader = new ModuleLoader();

        if (!services.IsAdded<IConfiguration>())
        {
            services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(
                    applicationCreationOptions.Configuration
                )
            );
        }

       

        services.AddServiceProviderHook<PropertyInitializerServiceProviderHook>(ServiceLifetime.Scoped);
        services.TryAddSingleton<IBootstrapLoggerFactory>(new DefaultBootstrapLoggerFactory());
        services.AddTransient<OnAppInitModuleLifecycleContributor>();
        services.AddTransient<OnAppShutdownModuleLifecycleContributor>();
        services.AddSingleton<IModuleManager, ModuleManager>();
        services.TryAddSingleton<IModuleLoader>(moduleLoader);

        services.Configure<ModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnAppInitModuleLifecycleContributor>();
            options.Contributors.Add<OnAppShutdownModuleLifecycleContributor>();
        });
    }
}
