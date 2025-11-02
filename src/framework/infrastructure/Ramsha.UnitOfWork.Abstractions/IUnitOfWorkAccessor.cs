namespace Ramsha.UnitOfWork.Abstractions;

public interface IUnitOfWorkAccessor
{
    IUnitOfWork? UnitOfWork { get; }
    void SetUnitOfWork(IUnitOfWork? unitOfWork);
}
