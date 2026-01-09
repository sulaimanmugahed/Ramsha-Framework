using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore.Mvc;

[CamelCaseControllerName]
[Route("[controller]")]
public abstract class RamshaControllerBase : ControllerBase
{
    protected IRamshaMediator Mediator => HttpContext.RequestServices.GetLazyRequiredService<IRamshaMediator>().Value;
    protected IUnitOfWorkManager UnitOfWorkManager => HttpContext.RequestServices.GetLazyRequiredService<IUnitOfWorkManager>().Value;
    protected IGlobalQueryFilterManager GlobalQueryFilterManager => HttpContext.RequestServices.GetLazyRequiredService<IGlobalQueryFilterManager>().Value;

    protected virtual async Task<RamshaActionResult> Query(IRamshaQuery query)
    {
        IRamshaResult result = await Mediator.Send(query);
        return RamshaResult(result);
    }

    protected virtual async Task<RamshaActionResult> Command(IRamshaCommand command)
    {
        IRamshaResult result = await Mediator.Send(command);
        return RamshaResult(result);
    }


    protected virtual RamshaActionResult RamshaResult(IRamshaResult result)
    {
        return new RamshaActionResult(result, HttpContext);
    }

    protected Task<T> TransactionalUnitOfWork<T>(Func<Task<T>> action)
    {
        return UnitOfWork(action, true);
    }
    protected Task TransactionalUnitOfWork(Func<Task> action)
    {
        return UnitOfWork(action, true);
    }


    protected async Task<T> UnitOfWork<T>(Func<Task<T>> action, bool isTransactional = false)
    {
        var options = new UnitOfWorkOptions();
        options.IsTransactional = isTransactional;

        if (UnitOfWorkManager.TryBeginReserved(
                RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName,
                options))
        {
            var result = await action();
            if (UnitOfWorkManager.Current is not null)
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return result;
        }

        using (var uow = UnitOfWorkManager.Begin(options))
        {
            var result = await action();
            await uow.CompleteAsync();
            return result;
        }
    }

    protected async Task UnitOfWork(Func<Task> action, bool isTransactional = false)
    {
        var options = new UnitOfWorkOptions();
        options.IsTransactional = isTransactional;

        if (UnitOfWorkManager.TryBeginReserved(
                RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName,
                options))
        {
            await action();
            if (UnitOfWorkManager.Current is not null)
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return;
        }

        using (var uow = UnitOfWorkManager.Begin(options))
        {
            await action();
            await uow.CompleteAsync();
        }
    }



}
