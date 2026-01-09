using Ramsha.Permissions.Api;
using Ramsha.Permissions.Application;
using Ramsha.Permissions.Persistence;

namespace Ramsha.Permissions;

public class PermissionsModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<PermissionsApplicationModule>()
        .DependsOn<PermissionsPersistenceModule>()
        .DependsOn<PermissionsApiModule>();
    }
}
