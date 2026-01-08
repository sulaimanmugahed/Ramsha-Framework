

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha;
using Ramsha.Common.Application;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddRamshaService<TServiceInterface, TService>(this IServiceCollection services)
    where TService : RamshaService, TServiceInterface
    {
        services.AddRamshaService(typeof(TService), typeof(TServiceInterface));
    }
    public static void AddRamshaService<TService>(this IServiceCollection services)
    where TService : RamshaService

    {
        services.AddRamshaService(typeof(TService));
    }
    public static void AddRamshaService(this IServiceCollection services, Type serviceType, Type? customServiceInterface = null)
    {
        if (!typeof(RamshaService).IsAssignableFrom(serviceType))
        {
            throw new Exception("invalid ramsha service type");
        }

        services.TryAddTransient(serviceType, provider => provider.CreateInstanceWithPropInjection(serviceType));
        if (customServiceInterface is not null)
        {
            services.TryAddTransient(customServiceInterface, p => p.GetRequiredService(serviceType));
        }
    }
    public static void AddRamshaService(this IServiceCollection services, Type serviceType)
    {
        if (!typeof(RamshaService).IsAssignableFrom(serviceType))
        {
            throw new Exception("invalid ramsha service type");
        }

        services.AddTransient(serviceType, provider => provider.CreateInstanceWithPropInjection(serviceType));
    }
    public static IServiceCollection AddCommonApplicationServices(this IServiceCollection services)
    {
        var appServiceTypes = RamshaAssemblyHelpers.GetAssembliesWithAccessTo(typeof(CommonApplicationModule))
.SelectMany(a => a.GetTypes())
.Where(t => t.IsClass &&
           !t.IsAbstract &&
           !t.IsGenericType &&
           typeof(RamshaService).IsAssignableFrom(t))
.ToList();

        foreach (var type in appServiceTypes)
        {
            services.AddRamshaService(type);
        }

        return services;
    }
}
