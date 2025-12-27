using LiteBus.Queries.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;


public interface IRamshaQuery<TResult> : IQuery<TResult>, IRamshaRequest
{

}
public interface IRamshaQuery : IRamshaQuery<IRamshaResult>
{

}

