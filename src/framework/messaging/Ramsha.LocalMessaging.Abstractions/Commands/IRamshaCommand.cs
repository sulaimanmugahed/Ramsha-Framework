using LiteBus.Commands.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaCommand : IRamshaCommand<IRamshaResult>
{

}

public interface IRamshaCommand<TResult> : ICommand<TResult>, IRamshaRequest
{

}

