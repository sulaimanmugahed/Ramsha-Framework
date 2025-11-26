using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ramsha.Common.Domain;
using Ramsha.Security.Users;

namespace Ramsha.EntityFrameworkCore;

public class SoftDeleteInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null) return result;

        var entries = context.ChangeTracker
        .Entries<ISoftDelete>()
        .Where(x => x.State == EntityState.Deleted)
        .ToList();

        foreach (var entry in entries)
        {
            entry.Entity.DeletionDate = DateTime.UtcNow;
            entry.Entity.DeletedBy = currentUser.Id;
            entry.State = EntityState.Modified;
        }

        return result;
    }
}
