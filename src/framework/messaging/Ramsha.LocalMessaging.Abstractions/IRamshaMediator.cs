

namespace Ramsha.LocalMessaging.Abstractions;

public interface IRamshaMediator
{
        Task<TQueryResult> Send<TQueryResult>(IRamshaQuery<TQueryResult> query, CancellationToken cancellationToken = default);
        Task<IRamshaResult> Send(IRamshaQuery query, CancellationToken cancellationToken = default);
        Task<IRamshaResult> Send(IRamshaCommand command, CancellationToken cancellationToken = default);
        Task<TCommandResult> Send<TCommandResult>(IRamshaCommand<TCommandResult> command, CancellationToken cancellationToken = default);

}
