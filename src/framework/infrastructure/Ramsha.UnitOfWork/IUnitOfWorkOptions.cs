namespace Ramsha.UnitOfWork;

public interface IUnitOfWorkOptions
{
    bool IsTransactional { get; }
}
