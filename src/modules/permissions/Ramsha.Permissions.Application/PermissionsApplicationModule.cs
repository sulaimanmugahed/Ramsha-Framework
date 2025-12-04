using Microsoft.Extensions.DependencyInjection;
using Ramsha.ApplicationAbstractions;
using Ramsha.Permissions.Contracts;
using Ramsha.Permissions.Domain;

namespace Ramsha.Permissions.Application;

public class PermissionsApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<PermissionsContractsModule>()
        .DependsOn<PermissionsDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddPermissionsApplicationServices();
    }
}
