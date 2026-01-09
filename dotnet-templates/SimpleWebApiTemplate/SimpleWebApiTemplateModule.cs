using Ramsha;
using Ramsha.AspNetCore.Mvc;
#if (useDatabase)
using SimpleWebApiTemplate.Data;
#endif
#if (useSqlServer)
using Ramsha.EntityFrameworkCore.SqlServer;
#endif

namespace SimpleWebApiTemplate;

public class SimpleWebApiTemplateModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<AspNetCoreMvcModule>();
#if (useSqlServer)
        ctx.DependsOn<EntityFrameworkCoreSqlServerModule>();
#endif
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
#if (useDatabase)
        context.Services.AddRamshaDbContext<SimpleWebApiTemplateDbContext>();
#endif
    }
}
