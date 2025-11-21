using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.Common.Domain;


public abstract class RamshaDomainManager
{
    [Injectable]
    public IServiceProvider ServiceProvider { get; set; } = default!;
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetLazyService<IUnitOfWorkManager>().Value;



    protected Task<T> TransactionalUnitOfWork<T>(Func<Task<T>> action)
    {
        return UnitOfWork(action, true);
    }
    protected Task TransactionalUnitOfWork(Func<Task> action)
    {
        return UnitOfWork(action, true);
    }

    protected async Task<T> UnitOfWork<T>(
     Func<Task<T>> action, bool isTransactional = false)
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