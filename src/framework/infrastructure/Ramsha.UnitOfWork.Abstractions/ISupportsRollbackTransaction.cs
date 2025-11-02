namespace Ramsha.UnitOfWork.Abstractions;

public interface ISupportsRollbackTransaction : ITransactionApi
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
