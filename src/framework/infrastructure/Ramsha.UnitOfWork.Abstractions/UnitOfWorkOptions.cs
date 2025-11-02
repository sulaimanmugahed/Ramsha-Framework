namespace Ramsha.UnitOfWork.Abstractions;

public class UnitOfWorkOptions : IUnitOfWorkOptions
{
    public bool IsTransactional { get; set; }
}
