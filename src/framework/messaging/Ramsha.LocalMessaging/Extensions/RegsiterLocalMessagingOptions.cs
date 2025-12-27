
using LiteBus.Events;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;

using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void RegisterLocalMessagingOptions(this IServiceCollection services, LocalMessagingOptions options)
    {
        var assemblies = options.Assemblies;
        services.AddLiteBus(builder =>
        {
            builder.AddRamshaCommandModule(module =>
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

            builder.AddRamshaQueryModule(module =>
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
