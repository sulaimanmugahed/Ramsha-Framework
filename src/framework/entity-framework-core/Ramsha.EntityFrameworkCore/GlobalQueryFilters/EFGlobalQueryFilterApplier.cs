using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ramsha.EntityFrameworkCore;

public class EFGlobalQueryFilterApplier<TDbContext>
where TDbContext : RamshaEFDbContext<TDbContext>
{
    private readonly IEnumerable<IEFGlobalQueryFilterProvider<TDbContext>> _filters;

    public EFGlobalQueryFilterApplier(IEnumerable<IEFGlobalQueryFilterProvider<TDbContext>> filters)
    {
        _filters = filters;
    }

    private bool ShouldApplyFilterForEntity(Type filterType, Type entityType)
    {
        return filterType.IsAssignableFrom(entityType) || entityType == filterType;
    }

    public void ApplyFilters(ModelBuilder modelBuilder, TDbContext dbContext)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;
            var entityBuilder = modelBuilder.Entity(entityClrType);

            foreach (var filter in _filters)
            {
                if (!ShouldApplyFilterForEntity(filter.FilterType, entityClrType))
                {
                    continue;
                }
                var lambda = filter.GetFilter(dbContext);
                if (lambda != null)
                {
                    if (entityClrType == filter.FilterType)
                    {
                        entityBuilder.HasQueryFilter(lambda);
                    }
                    else
                    {
                        var convertedLambda = ExpressionHelpers.ConvertToEntityType(
                    lambda,
                    entityClrType
                );
                        entityBuilder.HasQueryFilter(convertedLambda);
                    }

                }

            }
        }
    }
}
