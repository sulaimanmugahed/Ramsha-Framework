

using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha;
using Ramsha.Settings;
using Ramsha.SettingsManagement.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettingsManagementDomainServices(this IServiceCollection services)
    {
        services.AddRamshaDomainManager<ISettingManager, SettingManager>();

        services.AddTransient<ISettingManagerStore, SettingManagerStore>();
        services.Replace(ServiceDescriptor.Transient(typeof(ISettingStore), typeof(PersistSettingStore)));


        var settingManagerResolverInterfaceType = typeof(ISettingManagerResolver);

        var permissionResolvers = RamshaTypeHelpers.GetImplementationTypes<SettingsModule>(settingManagerResolverInterfaceType);
        foreach (var resolver in permissionResolvers)
        {
            services.AddTransient(settingManagerResolverInterfaceType, resolver);
            services.AddTransient(resolver);
        }


        services.Configure<SettingsManagementOptions>(options =>
        {
            options.ManagerResolvers.Add<UserSettingManagerResolver>();
        });

        return services;
    }
}
