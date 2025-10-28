using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.LocalMessaging;

public class RamshaMediator(IQueryMediator queryMediator, ICommandMediator commandMediator) : IRamshaMediator
{
    public Task Send(IRamshaCommand command, CancellationToken cancellationToken = default)
    {
        return commandMediator.SendAsync(command, cancellationToken);
    }

    public Task<TCommandResult> Send<TCommandResult>(IRamshaCommand<TCommandResult> command, CancellationToken cancellationToken = default)
    {
        return commandMediator.SendAsync(command, cancellationToken);
    }

    public Task<TQueryResult> Send<TQueryResult>(IRamshaQuery<TQueryResult> query, CancellationToken cancellationToken = default)
    {
        return queryMediator.QueryAsync(query, cancellationToken);
    }
}
