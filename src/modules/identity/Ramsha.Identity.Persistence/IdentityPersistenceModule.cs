using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence.Extensions;

namespace Ramsha.Identity.Persistence;

public class IdentityPersistenceModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityDomainModule>();

        moduleBuilder.PreConfigure<RamshaIdentityOptions>(options =>
        {
            options.AddEntityFrameworkCore();
        });
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
    }

}
