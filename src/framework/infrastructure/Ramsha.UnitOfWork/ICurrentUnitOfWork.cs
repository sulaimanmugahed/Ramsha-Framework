namespace Ramsha.UnitOfWork;

public interface ICurrentUnitOfWork : IUnitOfWorkAccessor
{
    IUnitOfWork? GetActive();
}
