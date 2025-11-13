using Ramsha.Identity.Core;

namespace Ramsha.Identity.Contracts;

public class IdentityContractsModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityCoreModule>();

        moduleBuilder.OnCreatingConfigure<RamshaIdentityContractsOptions>(options => { });
    }
}
