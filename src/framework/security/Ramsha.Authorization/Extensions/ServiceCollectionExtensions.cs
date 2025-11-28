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

        services.AddSingleton<IPermissionStore, InMemoryUserPermissionStore>();

        services.AddSingleton<IPermissionChecker, PermissionChecker>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, RamshaAuthorizationPolicyProvider>();


        var providerInterfaceType = typeof(IPermissionDefinitionProvider);

        var providers = RamshaAssemblyHelpers.GetAssemblies(typeof(AuthorizationModule))
   .SelectMany(a => a.GetTypes())
   .Where(t => t.IsClass &&
               !t.IsAbstract && typeof(IPermissionDefinitionProvider).IsAssignableFrom(t))
   .ToList();

        foreach (var provider in providers)
        {
            services.AddTransient(providerInterfaceType, provider);
        }


        return services;
    }
}
