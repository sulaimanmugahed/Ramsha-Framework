using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Commands;
using LiteBus.Events;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.LocalMessaging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void RegisterLocalMessagingOptions(this IServiceCollection services, LocalMessagingOptions options)
    {
        var assemblies = options.Assemblies;
        services.AddLiteBus(builder =>
        {
            builder.AddCommandModule(module =>
            {
                
                foreach (var handler in options.CommandHandlers)
                {
                    module.Register(handler);
                }
                foreach (var assembly in assemblies)
                {
                    module.RegisterFromAssembly(assembly);
                }

            });

            builder.AddQueryModule(module =>
          {
              foreach (var assembly in assemblies)
              {
                  module.RegisterFromAssembly(assembly);
              }
          });

            builder.AddEventModule(module =>
          {
              foreach (var assembly in assemblies)
              {
                  module.RegisterFromAssembly(assembly);
              }
          });
        });
    }
}
