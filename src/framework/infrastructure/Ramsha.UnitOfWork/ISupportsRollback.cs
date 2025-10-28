namespace Ramsha.UnitOfWork;

public interface ISupportsRollback
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
