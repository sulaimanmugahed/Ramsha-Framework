using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.LocalMessaging;

public class UnitOfWorkLocalBus(ILocalBus localBus) : IUnitOfWorkLocalEventBus
{
    public async Task Publish(UoWLocalEvent uoWEvent, CancellationToken cancellationToken = default)
    {
        await localBus.Publish(uoWEvent.Data, null, cancellationToken);
    }

    public async Task PublishMany(IEnumerable<UoWLocalEvent> uoWEvents, CancellationToken cancellationToken = default)
    {
        foreach (var ev in uoWEvents)
        {
            await Publish(ev, cancellationToken);
        }
    }
}
