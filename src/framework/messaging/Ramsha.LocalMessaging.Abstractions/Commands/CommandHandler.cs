namespace Ramsha.LocalMessaging.Abstractions;

public abstract class CommandHandler<TCommand, TResult> : IRamshaCommandHandler<TCommand, TResult>
where TCommand : IRamshaCommand<TResult>
{
    public abstract Task<TResult> HandleAsync(TCommand message, CancellationToken cancellationToken = default);
}

public abstract class CommandHandler<TCommand> : IRamshaCommandHandler<TCommand>
where TCommand : IRamshaCommand
{
    public abstract Task HandleAsync(TCommand message, CancellationToken cancellationToken = default);
}

