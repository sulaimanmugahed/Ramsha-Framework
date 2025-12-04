using Microsoft.Extensions.DependencyInjection;
using Ramsha.Authorization;
using Ramsha.Common.Domain;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Domain;

public class PermissionsDomainModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<PermissionsSharedModule>()
        .DependsOn<CommonDomainModule>()
        .DependsOn<AuthorizationModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddPermissionsDomainServices();
    }
}
