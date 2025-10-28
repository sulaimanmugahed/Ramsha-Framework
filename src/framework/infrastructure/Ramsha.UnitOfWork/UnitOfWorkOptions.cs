namespace Ramsha.UnitOfWork;

public class UnitOfWorkOptions : IUnitOfWorkOptions
{
    public bool IsTransactional { get; set; }
}
