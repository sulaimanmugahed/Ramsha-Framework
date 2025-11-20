
using Ramsha.Identity.Shared;

namespace Ramsha.Identity.Contracts;

public class IdentityContractsModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentitySharedModule>();

        moduleBuilder.OnCreatingConfigure<RamshaIdentityContractsOptions>(options => { });
    }
}
