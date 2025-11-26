using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.EntityFrameworkCore.SqlServer;

public class EntityFrameworkCoreSqlServerModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<EntityFrameworkCoreModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.Configure<RamshaDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseSqlServer();
            });
        });
    }
}
