using CleanWebApiTemplate.Domain;
using Microsoft.Extensions.DependencyInjection;
using Ramsha;
#if (useSqlServer)
using Ramsha.EntityFrameworkCore.SqlServer;
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
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddCleanWebApiTemplatePersistenceServices();
    }
}