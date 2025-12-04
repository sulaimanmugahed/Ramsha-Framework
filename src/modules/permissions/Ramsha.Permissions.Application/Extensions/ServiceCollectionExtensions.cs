using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Permissions.Application;
using Ramsha.Permissions.Contracts;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionsApplicationServices(this IServiceCollection services)
    {
        services.AddRamshaService<IPermissionsService, PermissionsService>();
        return services;
    }
}
