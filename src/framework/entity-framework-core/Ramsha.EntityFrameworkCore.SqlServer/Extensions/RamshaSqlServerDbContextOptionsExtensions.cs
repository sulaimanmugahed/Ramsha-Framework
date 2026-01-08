
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ramsha.EntityFrameworkCore.SqlServer;

public static class RamshaSqlServerDbContextOptionsExtensions
{
    public static void UseSqlServer(
        this RamshaDbContextOptions options,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseSqlServer(sqlServerOptionsAction);
        });
    }

    public static void UseSqlServer<TDbContext>(
         this RamshaDbContextOptions options,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
        where TDbContext : RamshaEFDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseSqlServer(sqlServerOptionsAction);
        });
    }

}
