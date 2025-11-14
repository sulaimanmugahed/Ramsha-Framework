using Microsoft.AspNetCore.Mvc;
using Ramsha.Core;
using Ramsha.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore.Mvc;


public abstract class RamshaControllerBase : ControllerBase
{
    protected IRamshaMediator Mediator => HttpContext.RequestServices.GetLazyRequiredService<IRamshaMediator>().Value;
    protected IUnitOfWorkManager UnitOfWorkManager => HttpContext.RequestServices.GetLazyRequiredService<IUnitOfWorkManager>().Value;
    protected IGlobalQueryFilterManager GlobalQueryFilterManager => HttpContext.RequestServices.GetLazyRequiredService<IGlobalQueryFilterManager>().Value;


    protected async Task<RamshaResult> BeginUnitOfWork(Func<Task<RamshaResult>> action)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName,
                new UnitOfWork.Abstractions.UnitOfWorkOptions()))
        {
            return await action();
        }

        return RamshaResult.Failure();
    }

    protected async Task<RamshaResult<T>> BeginUnitOfWork<T>(Func<Task<RamshaResult<T>>> action)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName,
                new UnitOfWork.Abstractions.UnitOfWorkOptions()))
        {
            return await action();
        }

        return RamshaResult<T>.Failure();
    }

}
