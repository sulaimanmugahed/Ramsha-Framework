

using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Ramsha.EntityFrameworkCore.PostgreSql;

public static class RamshaPostgreSqlDbContextConfigurationContextExtensions
{
    public static DbContextOptionsBuilder UsePostgreSql(
          this RamshaDbContextConfigurationContext context,
          Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseNpgsql(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                postgreSqlOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseNpgsql(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                postgreSqlOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}
