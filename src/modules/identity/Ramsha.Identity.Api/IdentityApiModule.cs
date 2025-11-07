using Ramsha.Identity.Contracts;

namespace Ramsha.Identity.Api;

public class IdentityApiModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityContractsModule>();
    }
}
