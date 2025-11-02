namespace Ramsha.UnitOfWork.Abstractions;

public interface ICurrentUnitOfWork : IUnitOfWorkAccessor
{
    IUnitOfWork? GetActive();
}
