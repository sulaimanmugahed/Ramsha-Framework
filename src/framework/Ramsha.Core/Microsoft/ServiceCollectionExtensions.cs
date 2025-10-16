using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Ramsha;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IRamshaAppWithExternalServiceProvider AddApplication<TStartupModule>(
     [NotNull] this IServiceCollection services,
     Action<AppCreationOptions>? optionsAction = null)
     where TStartupModule : IRamshaModule
    {
        return AppFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static IRamshaAppWithExternalServiceProvider AddApplication(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        Action<AppCreationOptions>? optionsAction = null)
    {
        return AppFactory.Create(startupModuleType, services, optionsAction);
    }

    public async static Task<IRamshaAppWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        [NotNull] this IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null)
        where TStartupModule : IRamshaModule
    {
        return await AppFactory.CreateAsync<TStartupModule>(services, optionsAction);
    }

    public async static Task<IRamshaAppWithExternalServiceProvider> AddApplicationAsync(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        Action<AppCreationOptions>? optionsAction = null)
    {
        return await AppFactory.CreateAsync(startupModuleType, services, optionsAction);
    }

    public static string? GetApplicationName(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IAppInfoAccessor>().ApplicationName;
    }

    public static string GetApplicationInstanceId(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IAppInfoAccessor>().InstanceId;
    }

    public static IRamshaHostEnvironment GetRamshaHostEnvironment(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IRamshaHostEnvironment>();
    }



    public static Type? NormalizedImplementationType(this ServiceDescriptor descriptor)
    {
        return descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType;
    }

    public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor)
    {
        return descriptor.IsKeyedService ? descriptor.KeyedImplementationInstance : descriptor.ImplementationInstance;
    }

    public static bool IsAdded<T>(this IServiceCollection services)
    {
        return services.IsAdded(typeof(T));
    }

    public static bool IsAdded(this IServiceCollection services, Type type)
    {
        return services.Any(d => d.ServiceType == type);
    }

    public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T?)services
            .FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.NormalizedImplementationInstance();
    }

    public static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();
        if (service == null)
        {
            throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
        }

        return service;
    }

    public static IServiceProvider BuildServiceProviderFromFactory([NotNull] this IServiceCollection services)
    {

        foreach (var service in services)
        {
            var factoryInterface = service.NormalizedImplementationInstance()?.GetType()
                .GetTypeInfo()
                .GetInterfaces()
                .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

            if (factoryInterface == null)
            {
                continue;
            }

            var containerBuilderType = factoryInterface.GenericTypeArguments[0];
            return (IServiceProvider)typeof(ServiceCollectionExtensions)
                .GetTypeInfo()
                .GetMethods()
                .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                .MakeGenericMethod(containerBuilderType)
                .Invoke(null, new object?[] { services, null })!;
        }

        return services.BuildServiceProvider();
    }

    public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>([NotNull] this IServiceCollection services, Action<TContainerBuilder>? builderAction = null) where TContainerBuilder : notnull
    {

        var serviceProviderFactory = services.GetSingletonInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>();
        if (serviceProviderFactory == null)
        {
            throw new Exception($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
        }

        var builder = serviceProviderFactory.CreateBuilder(services);
        builderAction?.Invoke(builder);
        return serviceProviderFactory.CreateServiceProvider(builder);
    }

    /// <summary>
    /// Resolves a dependency using given <see cref="IServiceCollection"/>.
    /// This method should be used only after dependency injection registration phase completed.
    /// </summary>
    internal static T? GetService<T>(this IServiceCollection services)
    {
        return services
            .GetSingletonInstance<IRamshaApp>()
            .ServiceProvider
            .GetService<T>();
    }


    internal static object? GetService(this IServiceCollection services, Type type)
    {
        return services
            .GetSingletonInstance<IRamshaApp>()
            .ServiceProvider
            .GetService(type);
    }


    public static T GetRequiredService<T>(this IServiceCollection services) where T : notnull
    {
        return services
            .GetSingletonInstance<IRamshaApp>()
            .ServiceProvider
            .GetRequiredService<T>();
    }


    public static object GetRequiredService(this IServiceCollection services, Type type)
    {
        return services
            .GetSingletonInstance<IRamshaApp>()
            .ServiceProvider
            .GetRequiredService(type);
    }

    public static Lazy<T?> GetServiceLazy<T>(this IServiceCollection services)
    {
        return new Lazy<T?>(services.GetService<T>, true);
    }

    public static Lazy<object?> GetServiceLazy(this IServiceCollection services, Type type)
    {
        return new Lazy<object?>(() => services.GetService(type), true);
    }


    public static Lazy<T> GetRequiredServiceLazy<T>(this IServiceCollection services) where T : notnull
    {
        return new Lazy<T>(services.GetRequiredService<T>, true);
    }


    public static Lazy<object> GetRequiredServiceLazy(this IServiceCollection services, Type type)
    {
        return new Lazy<object>(() => services.GetRequiredService(type), true);
    }

    public static IServiceProvider? GetServiceProviderOrNull(this IServiceCollection services)
    {
        return services.GetObjectOrNull<IServiceProvider>();
    }

    public static T? GetSingletonOrNull<T>(this IServiceCollection services)
    {
        return (T?)services.FirstOrDefault(d => d.ServiceType == typeof(T))
        ?.NormalizedImplementationInstance();
    }

    public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Replace(ServiceDescriptor.Singleton<IConfiguration>(configuration));
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services.GetConfigurationOrNull() ??
               throw new Exception("Could not find an implementation of " + typeof(IConfiguration).AssemblyQualifiedName + " in the service collection.");
    }

    public static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
        if (hostBuilderContext?.Configuration != null)
        {
            return hostBuilderContext.Configuration as IConfigurationRoot;
        }

        return services.GetSingletonInstanceOrNull<IConfiguration>();
    }
}
