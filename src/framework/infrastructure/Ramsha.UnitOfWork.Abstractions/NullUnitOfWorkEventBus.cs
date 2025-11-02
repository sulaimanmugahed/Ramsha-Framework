using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.UnitOfWork.Abstractions;

public class NullUnitOfWorkEventBus : IUnitOfWorkLocalEventBus
{
    public Task Publish(UoWLocalEvent uoWEvent, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task PublishMany(IEnumerable<UoWLocalEvent> uoWEvents, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}