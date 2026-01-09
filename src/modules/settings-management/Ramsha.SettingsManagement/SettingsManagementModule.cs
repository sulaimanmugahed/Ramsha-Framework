using Ramsha.SettingsManagement.Api;
using Ramsha.SettingsManagement.Application;
using Ramsha.SettingsManagement.Persistence;

namespace Ramsha.SettingsManagement;

public class SettingsManagementModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<SettingsManagementApplicationModule>()
        .DependsOn<SettingsManagementPersistenceModule>()
        .DependsOn<SettingsManagementApiModule>();
    }
}
