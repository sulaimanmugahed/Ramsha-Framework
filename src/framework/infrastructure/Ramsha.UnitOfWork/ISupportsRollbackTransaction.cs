namespace Ramsha.UnitOfWork;

public interface ISupportsRollbackTransaction : ITransactionApi
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
