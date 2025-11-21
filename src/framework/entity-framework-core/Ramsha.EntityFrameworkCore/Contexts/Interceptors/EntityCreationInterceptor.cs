
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ramsha.Common.Domain;

namespace Ramsha.EntityFrameworkCore;

public class EntityCreationInterceptor : SaveChangesInterceptor
{
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null) return result;

        var entries = context.ChangeTracker
        .Entries<IEntityCreation>()
        .Where(x => x.State == EntityState.Added)
        .ToList();

        foreach (var entry in entries)
        {
            entry.Entity.CreationDate = DateTime.UtcNow;
        }

        return result;
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
