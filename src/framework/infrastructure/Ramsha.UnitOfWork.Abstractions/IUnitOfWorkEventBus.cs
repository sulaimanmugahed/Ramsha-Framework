using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.UnitOfWork.Abstractions;

public interface IUnitOfWorkLocalEventBus
{
    Task Publish(UoWLocalEvent uoWEvent, CancellationToken cancellationToken = default);
    Task PublishMany(IEnumerable<UoWLocalEvent> uoWEvents, CancellationToken cancellationToken = default);
}


