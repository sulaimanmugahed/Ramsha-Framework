using Ramsha.Identity.Contracts;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Application;

public class IdentityApplicationModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder
        .DependsOn<IdentityContractsModule>()
        .DependsOn<IdentityDomainModule>();
    }
}
