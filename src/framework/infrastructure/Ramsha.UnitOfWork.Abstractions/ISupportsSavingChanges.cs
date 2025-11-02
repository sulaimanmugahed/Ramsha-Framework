namespace Ramsha.UnitOfWork.Abstractions;

public interface ISupportsSavingChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
