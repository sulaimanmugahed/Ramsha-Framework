namespace Ramsha.UnitOfWork.Abstractions;

public interface ISupportsRollback
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
