namespace Ramsha.UnitOfWork.Abstractions;

public interface IUnitOfWorkOptions
{
    bool IsTransactional { get; }
}
