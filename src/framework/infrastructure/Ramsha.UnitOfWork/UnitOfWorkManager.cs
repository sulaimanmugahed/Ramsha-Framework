using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    public IUnitOfWork? Current => _ambientUnitOfWork.GetActive();

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICurrentUnitOfWork _ambientUnitOfWork;

    public UnitOfWorkManager(IServiceScopeFactory serviceScopeFactory, ICurrentUnitOfWork ambientUnitOfWork)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _ambientUnitOfWork = ambientUnitOfWork;
    }

    public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
    {
        var currentUow = Current;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }

        var unitOfWork = CreateNewUnitOfWork();
        unitOfWork.Initialize(options);

        return unitOfWork;
    }

    private IUnitOfWork CreateNewUnitOfWork()
    {
        var scope = _serviceScopeFactory.CreateScope();
        try
        {
            var outerUow = _ambientUnitOfWork.UnitOfWork;

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.SetOuter(outerUow);

            _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

            return unitOfWork;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }

    public IUnitOfWork Reserve(string reservationName, bool requiresNew = false)
    {

        if (!requiresNew &&
            _ambientUnitOfWork.UnitOfWork != null &&
            _ambientUnitOfWork.UnitOfWork.IsReservedFor(reservationName))
        {
            return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
        }

        var unitOfWork = CreateNewUnitOfWork();
        unitOfWork.Reserve(reservationName);

        return unitOfWork;
    }

    public void BeginReserved(string reservationName, UnitOfWorkOptions options)
    {
        if (!TryBeginReserved(reservationName, options))
        {
            throw new Exception($"Could not find a reserved unit of work with reservation name: {reservationName}");
        }
    }

    public bool TryBeginReserved(string reservationName, UnitOfWorkOptions options)
    {

        var uow = _ambientUnitOfWork.UnitOfWork;

        while (uow != null && !uow.IsReservedFor(reservationName))
        {
            uow = uow.Outer;
        }

        if (uow == null)
        {
            return false;
        }

        uow.Initialize(options);

        return true;
    }
}
