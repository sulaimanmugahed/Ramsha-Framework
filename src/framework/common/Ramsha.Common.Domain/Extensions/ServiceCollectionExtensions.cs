
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha;
using Ramsha.Common.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddRamshaDomainManager<TManagerInterface, TManager>(this IServiceCollection services)
    where TManager : RamshaDomainManager, TManagerInterface
    {
        services.AddRamshaDomainManager(typeof(TManager), typeof(TManagerInterface));
    }
    public static void AddRamshaDomainManager<TManager>(this IServiceCollection services)
    where TManager : RamshaDomainManager
    {
        services.AddRamshaDomainManager(typeof(TManager));
    }
    public static void AddRamshaDomainManager(this IServiceCollection services, Type managerType, Type? customManagerInterface = null)
    {
        if (!typeof(RamshaDomainManager).IsAssignableFrom(managerType))
        {
            throw new Exception("invalid ramsha service type");
        }

        services.TryAddTransient(managerType, provider => provider.CreateInstanceWithPropInjection(managerType));

        if (customManagerInterface is not null)
        {
            services.TryAddTransient(customManagerInterface, p => p.GetRequiredService(managerType));
        }
    }
    public static IServiceCollection AddCommonDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IGlobalQueryFilterManager, GlobalQueryFilterManager>();

        services.AddSingleton(typeof(IGlobalQueryFilterManager<>), typeof(GlobalQueryFilterManager<>));


        var managerTypes = RamshaAssemblyHelpers.GetAssembliesWithAccessTo(typeof(CommonDomainModule))
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsClass &&
                !t.IsAbstract &&
                !t.IsGenericType &&
                typeof(RamshaDomainManager).IsAssignableFrom(t))
    .ToList();

        foreach (var type in managerTypes)
        {
            services.AddRamshaDomainManager(type);
        }

        return services;
    }

}
