using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Contracts;

public class SettingsManagementContractsModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<SettingsManagementSharedModule>();
    }
}
