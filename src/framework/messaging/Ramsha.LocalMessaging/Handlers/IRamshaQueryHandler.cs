using LiteBus.Queries.Abstractions;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.LocalMessaging;

public interface IRamshaQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>, IRamshaRequestHandler
       where TQuery : IRamshaQuery<TResult>
{

}

public interface IRamshaQueryHandler<TQuery> : IRamshaQueryHandler<TQuery, IRamshaResult>
where TQuery : IRamshaQuery<IRamshaResult>
{

}

