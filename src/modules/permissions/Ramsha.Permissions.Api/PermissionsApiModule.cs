using Ramsha.AspNetCore.Mvc;
using Ramsha.Permissions.Contracts;

namespace Ramsha.Permissions.Api;

public class PermissionsApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<PermissionsContractsModule>()
        .DependsOn<AspNetCoreMvcModule>();
    }
}
