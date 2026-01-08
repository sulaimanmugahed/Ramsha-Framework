
using LiteBus.Commands;
using LiteBus.Messaging;
using LiteBus.Queries;
using LiteBus.Runtime.Abstractions;

namespace Ramsha.LocalMessaging;

public static class RamshaModulesRegistryExtensions
{

    public static IModuleRegistry AddRamshaQueryModule(this IModuleRegistry moduleRegistry, Action<QueryModuleBuilder> builderAction)
    {
        ArgumentNullException.ThrowIfNull(moduleRegistry);
        ArgumentNullException.ThrowIfNull(builderAction);

        if (!moduleRegistry.IsModuleRegistered<MessageModule>())
        {
            moduleRegistry.Register(new MessageModule(_ =>
            {
            }));
        }

        moduleRegistry.Register(new RamshaQueryModule(builderAction));
        return moduleRegistry;
    }
    public static IModuleRegistry AddRamshaCommandModule(this IModuleRegistry moduleRegistry, Action<CommandModuleBuilder> builderAction)
    {
        ArgumentNullException.ThrowIfNull(moduleRegistry);
        ArgumentNullException.ThrowIfNull(builderAction);

        if (!moduleRegistry.IsModuleRegistered<MessageModule>())
        {
            moduleRegistry.Register(new MessageModule(_ =>
            {
            }));
        }

        moduleRegistry.Register(new RamshaCommandModule(builderAction));
        return moduleRegistry;
    }
}
