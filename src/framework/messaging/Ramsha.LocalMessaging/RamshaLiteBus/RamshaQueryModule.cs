using LiteBus.Messaging.Abstractions;
using LiteBus.Messaging.Registry;
using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using LiteBus.Runtime.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.LocalMessaging;

public sealed class RamshaQueryModule : IModule
{
    private readonly Action<QueryModuleBuilder> _builder;

    public RamshaQueryModule(Action<QueryModuleBuilder> builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public void Build(IModuleConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var messageRegistry = MessageRegistryAccessor.Instance;

        var startIndex = messageRegistry.Handlers.Count;
        var moduleBuilder = new QueryModuleBuilder(messageRegistry);
        _builder(moduleBuilder);

        RegisterQueryServices(configuration);
        RegisterNewHandlers(configuration, messageRegistry, startIndex);
    }

    private static void RegisterQueryServices(IModuleConfiguration configuration)
    {
        configuration.DependencyRegistry.Register(new DependencyDescriptor(
            typeof(IQueryMediator),
            typeof(QueryMediator)));
    }

    private static void RegisterNewHandlers(IModuleConfiguration configuration, IMessageRegistry messageRegistry, int startIndex)
    {
        var newHandlers = messageRegistry.Handlers.Skip(startIndex);

        foreach (var handlerDescriptor in newHandlers)
        {
            var handlerType = handlerDescriptor.HandlerType;

            if (handlerType is { IsClass: true, IsAbstract: false })
            {
                if (handlerType.IsGenericType)
                {
                    configuration.DependencyRegistry.Register(new DependencyDescriptor(
                    handlerType,
                    handlerType
                    ));
                }
                else
                {
                    configuration.DependencyRegistry.Register(new DependencyDescriptor(
                    handlerType,
                    sp => sp.CreateInstanceWithPropInjection(handlerType)
                    ));
                }
            }
        }
    }
}
