using LiteBus.Commands.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
where TCommand : IRamshaCommand<TResult>
{

}

public interface IRamshaCommandHandler<TCommand> : ICommandHandler<TCommand>
where TCommand : IRamshaCommand
{

}