using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Authorization;

public class AuthorizationModule : RamshaModule
{
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddAuthorizationServices();

        context.Configure<RamshaPermissionOptions>(options =>
        {
            options.PermissionResolvers.Add<UserPermissionProviderResolver>();
        });
    }


    public override void OnInit(InitContext context)
    {
        base.OnInit(context);


        var manager = context.ServiceProvider.GetRequiredService<PermissionDefinitionManager>();
        manager.Initialize();
    }
}
