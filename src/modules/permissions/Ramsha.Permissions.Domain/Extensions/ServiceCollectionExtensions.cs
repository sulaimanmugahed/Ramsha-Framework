using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Authorization;
using Ramsha.Permissions.Domain;
using Ramsha.Permissions.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionsDomainServices(this IServiceCollection services)
    {
        services.AddRamshaDomainManager<IPermissionManager, PermissionManager>();
        services.Replace(ServiceDescriptor.Transient(typeof(IPermissionStore), typeof(PersistPermissionStore)));
        return services;
    }
}
