using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;
using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Domain;

public class SettingsManagementDomainModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<SettingsManagementSharedModule>()
        .DependsOn<CommonDomainModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddSettingsManagementDomainServices();
    }
}
