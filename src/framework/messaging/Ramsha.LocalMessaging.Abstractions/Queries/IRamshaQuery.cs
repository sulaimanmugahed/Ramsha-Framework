using LiteBus.Queries.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;


public interface IRamshaQuery<TResult> : IQuery<TResult>, IRamshaQuery
{

}
public interface IRamshaQuery : IQuery
{

}

