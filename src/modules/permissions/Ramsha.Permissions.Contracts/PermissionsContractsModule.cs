using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Contracts;

public class PermissionsContractsModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<PermissionsSharedModule>();
    }
}
