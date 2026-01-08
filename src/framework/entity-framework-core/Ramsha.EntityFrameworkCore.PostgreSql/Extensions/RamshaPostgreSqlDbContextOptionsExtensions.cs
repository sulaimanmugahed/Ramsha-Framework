
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Ramsha.EntityFrameworkCore.PostgreSql;

public static class RamshaPostgreSqlDbContextOptionsExtensions
{
    public static void UsePostgreSql(
        this RamshaDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UsePostgreSql(postgreSqlOptionsAction);
        });
    }

    public static void UsePostgreSql<TDbContext>(
         this RamshaDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
        where TDbContext : RamshaEFDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UsePostgreSql(postgreSqlOptionsAction);
        });
    }

}
