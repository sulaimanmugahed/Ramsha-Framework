using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence.Extensions;

namespace Ramsha.Identity.Persistence;

public class IdentityPersistenceModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<IdentityDomainModule>()
        .DependsOn<EntityFrameworkCoreModule>();
    }
    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.Configure<RamshaIdentityOptions>(options =>
     {
         options.AddEntityFrameworkCore();
     });
    }
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
    }

}
