using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ramsha.EntityFrameworkCore;

public class EFGlobalQueryFilterApplier<TDbContext>
where TDbContext : RamshaEFDbContext<TDbContext>
{


    private bool ShouldApplyFilterForEntity(Type filterType, Type entityType)
    {
        return filterType.IsAssignableFrom(entityType) || entityType == filterType;
    }

    public void ApplyFilters(IEnumerable<IEFGlobalQueryFilterProvider<TDbContext>> filterProviders, Type entityClrType, EntityTypeBuilder entityTypeBuilder, TDbContext dbContext)
    {
        LambdaExpression? combined = null;

        foreach (var filter in filterProviders)
        {
            if (!ShouldApplyFilterForEntity(filter.FilterType, entityClrType))
                continue;

            var lambda = filter.GetFilter(dbContext);
            if (lambda == null)
                continue;

            var finalLambda = entityClrType == filter.FilterType
                ? lambda
                : ExpressionHelpers.ConvertToEntityType(lambda, entityClrType);

            combined = combined == null
                ? finalLambda
                : ExpressionHelpers.CombineExpressions(
                        (dynamic)combined,
                        (dynamic)finalLambda,
                        ExpressionHelpers.CombineOperator.And
                  );
        }

        if (combined != null)
            entityTypeBuilder.HasQueryFilter(combined);
    }



    public void ApplyFilters(IEnumerable<IEFGlobalQueryFilterProvider<TDbContext>> filterProviders, ModelBuilder modelBuilder, TDbContext dbContext)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;
            var entityBuilder = modelBuilder.Entity(entityClrType);
            ApplyFilters(filterProviders, entityClrType, entityBuilder, dbContext);
        }
    }
}
