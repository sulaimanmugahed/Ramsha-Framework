namespace Ramsha.UnitOfWork.Abstractions;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
