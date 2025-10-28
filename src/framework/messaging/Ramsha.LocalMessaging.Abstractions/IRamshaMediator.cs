using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Queries.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaMediator
{
        Task<TQueryResult> Send<TQueryResult>(IRamshaQuery<TQueryResult> query, CancellationToken cancellationToken = default);
        Task Send(IRamshaCommand command, CancellationToken cancellationToken = default);
        Task<TCommandResult> Send<TCommandResult>(IRamshaCommand<TCommandResult> command, CancellationToken cancellationToken = default);
}
