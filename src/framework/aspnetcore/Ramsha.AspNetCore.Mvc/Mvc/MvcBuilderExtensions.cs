using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.AspNetCore.Mvc;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddGenericControllers(this IMvcBuilder builder, params Type[] controllerType)
    {
        builder.ConfigureApplicationPartManager(c =>
          {
             c.FeatureProviders.Add(new GenericControllerFeatureProvider(controllerType.Select(x => x.GetTypeInfo()).ToArray()));
          });
        return builder;
    }
}