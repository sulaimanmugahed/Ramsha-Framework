using Microsoft.Extensions.DependencyInjection;
using Ramsha;

namespace Ramsha.LocalMessaging.Abstractions;

public abstract class QueryHandler<TQuery, TResult> : IRamshaQueryHandler<TQuery, TResult>, IHasPropertyInjection
    where TQuery : IRamshaQuery<TResult>
{

    public abstract Task<TResult> HandleAsync(TQuery message, CancellationToken cancellationToken = default);
}

