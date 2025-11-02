using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Events.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface ILocalEventHandler<TEvent> : IEventHandler<TEvent>
where TEvent : notnull
{

}

public abstract class LocalEventHandler<TEvent> : ILocalEventHandler<TEvent>
where TEvent : notnull
{
    public abstract Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}