using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ramsha.EntityFrameworkCore.Sqlite;

public static class RamshaSqliteDbContextOptionsExtensions
{
    public static void UseSqlite(
        this RamshaDbContextOptions options,
        Action<SqliteDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseSqlite(postgreSqlOptionsAction);
        });
    }

    public static void UseSqlite<TDbContext>(
         this RamshaDbContextOptions options,
        Action<SqliteDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
        where TDbContext : RamshaEFDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseSqlite(postgreSqlOptionsAction);
        });
    }

}
