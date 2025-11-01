using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ramsha.EntityFrameworkCore;

internal static class DbContextExtensions
{
    public static bool HasRelationalTransactionManager(this IEFDbContext dbContext)
    {
        return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
    }
}