using CleanWebApiTemplate.Domain;
using Microsoft.Extensions.DependencyInjection;
using Ramsha;
using Ramsha.EntityFrameworkCore.SqlServer;

namespace CleanWebApiTemplate.Persistence;

public class CleanWebApiTemplatePersistenceModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateDomainModule>()
        .DependsOn<EntityFrameworkCoreSqlServerModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddCleanWebApiTemplatePersistenceServices();
    }
}