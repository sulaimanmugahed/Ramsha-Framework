using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ramsha.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.EntityFrameworkCore;

public class DomainEventToUnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager) : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not null)
        {
            EnqueueEvents(context);
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private IEnumerable<IHasDomainEvent> GetEntitiesHasDomainEvents(DbContext context)
    {
        return context.ChangeTracker.Entries<IHasDomainEvent>()
                         .Select(e => e.Entity)
                         .Where(x => x.GetEvents().Any())
                         .ToArray();
    }

    private void EnqueueEvents(DbContext? context)
    {
        if (context == null) return;

        var entities = GetEntitiesHasDomainEvents(context);
        if (!entities.Any()) return;

        var uow = unitOfWorkManager.Current;
        if (uow is null) return;

        foreach (var entity in entities)
        {
            foreach (var e in entity.GetEvents())
            {
                uow.EnqueueLocalEvent(new UoWLocalEvent(e.Data));
            }
            entity.ClearEvents();
        }
    }
}
