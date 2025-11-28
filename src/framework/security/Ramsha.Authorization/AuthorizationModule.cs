using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Authorization;

public class AuthorizationModule : RamshaModule
{
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddAuthorizationServices();



    }


    public override void OnInit(InitContext context)
    {
        base.OnInit(context);


        var manager = context.ServiceProvider.GetRequiredService<PermissionDefinitionManager>();
        manager.Initialize();
    }
}
