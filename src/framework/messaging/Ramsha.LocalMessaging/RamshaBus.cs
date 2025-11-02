using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Events.Abstractions;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.LocalMessaging;

public class LocalBus(IEventMediator mediator) : ILocalBus
{
    public async Task Publish<TEvent>(TEvent @event, LocalBusPublishOptions? options = null, CancellationToken cancellationToken = default)
    where TEvent : notnull
    {
        await mediator.PublishAsync(@event, options is not null ? new EventMediationSettings
        {
            ThrowIfNoHandlerFound = options.ThrowIfNoHandler,
            Routing = new EventMediationRoutingSettings
            {
                Tags = options.Tags
            }
        } : null, cancellationToken);
    }

    public async Task PublishMany<TEvent>(List<TEvent> events, LocalBusPublishOptions? options = null, CancellationToken cancellationToken = default) where TEvent : notnull
    {
        foreach (var e in events)
        {
            await Publish(e, options, cancellationToken);
        }
    }
}
