using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.LocalMessaging.Abstractions;

public class LocalBusPublishOptions
{
    public List<string> Tags { get; set; } = [];
    public bool ThrowIfNoHandler { get; set; }
}

public interface ILocalBus
{
    Task Publish<TEvent>(TEvent @event, LocalBusPublishOptions? options = null, CancellationToken cancellationToken = default) where TEvent : notnull;
    Task PublishMany<TEvent>(List<TEvent> events, LocalBusPublishOptions? options = null, CancellationToken cancellationToken = default) where TEvent : notnull;

}
