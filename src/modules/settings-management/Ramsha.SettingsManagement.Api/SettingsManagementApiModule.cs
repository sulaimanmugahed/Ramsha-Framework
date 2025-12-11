using Ramsha.AspNetCore.Mvc;
using Ramsha.SettingsManagement.Contracts;

namespace Ramsha.SettingsManagement.Api;

public class SettingsManagementApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<SettingsManagementContractsModule>()
        .DependsOn<AspNetCoreMvcModule>();
    }
}
