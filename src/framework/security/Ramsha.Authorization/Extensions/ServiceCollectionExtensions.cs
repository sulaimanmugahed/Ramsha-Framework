using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Authorization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {

        services.AddTransient<PermissionDefinitionManager>();

        services.AddSingleton<IPermissionDefinitionStore, InMemoryPermissionDefinitionStore>();
        services.AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>();

        services.AddSingleton<IPermissionStore, InMemoryPermissionStore>();

        services.AddSingleton<IPermissionChecker, PermissionChecker>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, RamshaAuthorizationPolicyProvider>();

        services.AddSingleton<PermissionProviderResolversManager>();


        var permissionResolverInterfaceType = typeof(IPermissionProviderResolver);

        var permissionResolvers = RamshaTypeHelpers.GetRamshaTypes<AuthorizationModule>(permissionResolverInterfaceType);
        foreach (var resolver in permissionResolvers)
        {
            services.AddTransient(resolver);
        }






        var definitionProviderInterfaceType = typeof(IPermissionDefinitionProvider);
        var definitionProviderTypes = RamshaTypeHelpers.GetRamshaTypes<AuthorizationModule>(definitionProviderInterfaceType);
        foreach (var provider in definitionProviderTypes)
        {
            services.AddTransient(definitionProviderInterfaceType, provider);
        }


        return services;
    }
}
