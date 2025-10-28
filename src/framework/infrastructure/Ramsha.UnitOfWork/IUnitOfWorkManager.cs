namespace Ramsha.UnitOfWork;

public interface IUnitOfWorkManager
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false);
    IUnitOfWork Reserve(string reservationName, bool requiresNew = false);
    void BeginReserved(string reservationName, UnitOfWorkOptions options);
    bool TryBeginReserved(string reservationName, UnitOfWorkOptions options);
}
