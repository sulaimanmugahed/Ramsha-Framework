using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ramsha;
using Ramsha.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AutoRegisterPrincipalTransformers(this IServiceCollection services)
    {
        var assemblies = RamshaAssemblyHelpers.GetRamshaAssemblies();
        var modifierTypes = assemblies
       .SelectMany(a =>
       {
           try
           {
               return a.GetTypes();
           }
           catch (ReflectionTypeLoadException ex)
           {
               return ex.Types.Where(t => t != null)!;
           }
       })
       .Where(t => typeof(IRamshaClaimsPrincipalTransformer).IsAssignableFrom(t!)
                   && t!.IsClass
                   && !t.IsAbstract)
       .ToList()!;


        foreach (var type in modifierTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }


}
