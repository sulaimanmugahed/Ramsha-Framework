

using Ramsha.SettingsManagement.Domain;
using Ramsha.SettingsManagement.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettingsManagementPersistenceServices(this IServiceCollection services)
    {
        services.AddRamshaDbContext<ISettingsManagementDbContext, SettingsManagementDbContext>(options =>
        {
            options.AddRepository<Setting, ISettingRepository, EFSettingRepository>();
        });
        return services;
    }
}
