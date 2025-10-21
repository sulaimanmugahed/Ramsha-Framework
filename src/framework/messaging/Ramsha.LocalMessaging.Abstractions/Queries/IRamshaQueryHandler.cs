using LiteBus.Queries.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
       where TQuery : IRamshaQuery<TResult>
{

}

