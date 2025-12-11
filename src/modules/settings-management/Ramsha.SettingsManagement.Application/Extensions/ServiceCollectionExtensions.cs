

using Ramsha.SettingsManagement.Application;
using Ramsha.SettingsManagement.Contracts;
using Ramsha.SettingsManagement.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettingsManagementApplicationServices(this IServiceCollection services)
    {
        services.AddRamshaService<ISettingsService, SettingsService>();
        return services;
    }
}
