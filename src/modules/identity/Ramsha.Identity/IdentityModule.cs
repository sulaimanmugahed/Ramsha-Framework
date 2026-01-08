using Ramsha.Identity.Api;
using Ramsha.Identity.Application;
using Ramsha.Identity.Persistence;

namespace Ramsha.Identity;

public class IdentityModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<IdentityApplicationModule>()
        .DependsOn<IdentityPersistenceModule>()
        .DependsOn<IdentityApiModule>();
    }
}
