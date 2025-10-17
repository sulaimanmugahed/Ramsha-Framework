using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public static class AppFactory
{
    public async static Task<IRamshaAppWithInternalServiceProvider> CreateAsync<TStartupModule>(
        Action<AppCreationOptions>? optionsAction = null,
        IRamshaModule? startupModuleInstance = null)
        where TStartupModule : IRamshaModule
    {
        var app = Create(typeof(TStartupModule), options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IRamshaAppWithInternalServiceProvider> CreateAsync(
        [NotNull] Type startupModuleType,
        Action<AppCreationOptions>? optionsAction = null)
    {
        var app = new RamshaAppWithInternalServiceProvider(startupModuleType, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IRamshaAppWithExternalServiceProvider> CreateAsync<TStartupModule>(
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null)
        where TStartupModule : IRamshaModule
    {
        var app = Create(typeof(TStartupModule), services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IRamshaAppWithExternalServiceProvider> CreateAsync(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null,
         IRamshaModule? startupModuleInstance = null)
    {
        var app = new RamshaAppWithExternalServiceProvider(startupModuleType, services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        },startupModuleInstance);
        await app.ConfigureServicesAsync();
        return app;
    }

    public static IRamshaAppWithInternalServiceProvider Create<TStartupModule>(
        Action<AppCreationOptions>? optionsAction = null)
        where TStartupModule : IRamshaModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    public static IRamshaAppWithInternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        Action<AppCreationOptions>? optionsAction = null)
    {
        return new RamshaAppWithInternalServiceProvider(startupModuleType, optionsAction);
    }

    public static IRamshaAppWithExternalServiceProvider Create<TStartupModule>(
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null)
        where TStartupModule : IRamshaModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    public static IRamshaAppWithExternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null,
        IRamshaModule? startupModuleInstance = null
        )
    {
        return new RamshaAppWithExternalServiceProvider(startupModuleType, services, optionsAction,startupModuleInstance);
    }
}
