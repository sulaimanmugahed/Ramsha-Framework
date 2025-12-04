using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Permissions.Domain;
using Ramsha.Permissions.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionsPersistenceServices(this IServiceCollection services)
    {
        services.AddRamshaDbContext<PermissionsDbContext>(options =>
        {
            options.AddRepository<Permission, IPermissionRepository, EFCorePermissionRepository>();
        });

        return services;
    }
}
