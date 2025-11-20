
using Ramsha;
using Ramsha.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IGlobalQueryFilterManager, GlobalQueryFilterManager>();

        services.AddSingleton(typeof(IGlobalQueryFilterManager<>), typeof(GlobalQueryFilterManager<>));


        var managerTypes = RamshaAssemblyHelpers.GetRamshaAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsClass &&
                !t.IsAbstract &&
                !t.IsGenericType &&
                typeof(RamshaDomainManager).IsAssignableFrom(t))
    .ToList();

        foreach (var type in managerTypes)
        {
            services.AddTransient(type, provider => provider.CreateInstanceWithPropInjection(type));
        }

        return services;
    }

}
