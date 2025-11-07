using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

public class IdentityPersistenceModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityDomainModule>();
        moduleBuilder.PreConfigure<IdentityBuilder>(builder =>
        {
            builder.AddRamshaIdentityStore<IIdentityDbContext>();
        });
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddRamshaDbContext<IIdentityDbContext, IdentityDbContext>();
    }

}
