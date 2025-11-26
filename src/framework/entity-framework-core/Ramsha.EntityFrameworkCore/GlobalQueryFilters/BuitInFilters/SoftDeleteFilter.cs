using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ramsha.Common.Domain;

namespace Ramsha.EntityFrameworkCore;

public class SoftDeleteFilterProvider<TDbContext> : EFGlobalQueryFilterProvider<TDbContext, ISoftDelete>
where TDbContext : RamshaEFDbContext<TDbContext>
{
    protected override Expression<Func<ISoftDelete, bool>> CreateFilterExpression(TDbContext dbContext)
    {
        return softDelete => !softDelete.DeletionDate.HasValue;
    }
}
