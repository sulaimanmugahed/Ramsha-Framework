using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Application;
using Ramsha.SettingsManagement.Contracts;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Application;

public class SettingsManagementApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<SettingsManagementContractsModule>()
        .DependsOn<SettingsManagementDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddSettingsManagementApplicationServices();
    }
}
