using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Identity.Core.Options;

namespace Ramsha.Identity.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRamshaIdentityDomainServices(this IServiceCollection services)
    {
        var typesOptions = services.ExecutePreConfigured<RamshaIdentityTypesOptions>();

        // register userManager
        var ramshaUserManagerType = typeof(RamshaIdentityUserManager<,,,,,,,>).MakeGenericType(typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);

        services.TryAddScoped(ramshaUserManagerType);
        services.TryAddScoped(typeof(RamshaIdentityUserManager<>).MakeGenericType(typesOptions.UserType));
        services.TryAddScoped(typeof(UserManager<>).MakeGenericType(typesOptions.UserType), provider => provider.GetService(ramshaUserManagerType));

        // register roleManager
        var ramshaRoleManagerType = typeof(RamshaIdentityRoleManager<,,,>).MakeGenericType(typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType);
        services.TryAddScoped(ramshaRoleManagerType);
        services.TryAddScoped(typeof(RamshaIdentityRoleManager<>).MakeGenericType(typesOptions.RoleType));
        services.TryAddScoped(typeof(RoleManager<>).MakeGenericType(typesOptions.RoleType), provider => provider.GetService(ramshaRoleManagerType));

        return services;

    }
}
