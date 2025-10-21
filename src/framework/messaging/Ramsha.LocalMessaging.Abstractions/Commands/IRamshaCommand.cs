using LiteBus.Commands.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaCommand : ICommand
{

}

public interface IRamshaCommand<TResult> : ICommand<TResult>, IRamshaCommand
{

}

