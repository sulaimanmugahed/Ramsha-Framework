using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ramsha.Common.Domain;

namespace Ramsha.EntityFrameworkCore;

public class EntityModificationInterceptor : SaveChangesInterceptor
{
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null) return result;

        var entries = context.ChangeTracker
        .Entries<IEntityModification>()
        .Where(x => x.State == EntityState.Modified)
        .ToList();

        foreach (var entry in entries)
        {
            entry.Entity.LastUpdateDate ??= DateTime.UtcNow;
        }

        return result;
    }
}
