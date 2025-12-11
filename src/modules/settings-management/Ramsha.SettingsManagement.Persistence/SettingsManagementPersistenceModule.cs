using Microsoft.Extensions.DependencyInjection;
using Ramsha.EntityFrameworkCore;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Persistence;

public class SettingsManagementPersistenceModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<SettingsManagementDomainModule>()
        .DependsOn<EntityFrameworkCoreModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddSettingsManagementPersistenceServices();
    }


}
