using CleanWebApiTemplate.Domain;
using Microsoft.Extensions.DependencyInjection;
using Ramsha;
#if (useSqlServer)
using Ramsha.EntityFrameworkCore.SqlServer;
#endif
#if (usePostgres)
using Ramsha.EntityFrameworkCore.PostgreSql;
#endif
#if (useSqlite)
using Ramsha.EntityFrameworkCore.Sqlite;
#endif

namespace CleanWebApiTemplate.Persistence;

public class CleanWebApiTemplatePersistenceModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context.DependsOn<CleanWebApiTemplateDomainModule>();
#if (useSqlServer)
        context.DependsOn<EntityFrameworkCoreSqlServerModule>();
#endif
#if (usePostgres)
        context.DependsOn<EntityFrameworkCorePostgreSqlModule>();
#endif
#if (useSqlite)
        context.DependsOn<EntityFrameworkCoreSqliteModule>();
#endif
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddCleanWebApiTemplatePersistenceServices();
    }
}