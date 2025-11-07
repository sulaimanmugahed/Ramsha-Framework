using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Core;

namespace Ramsha.Identity.Domain;

public class IdentityDomainModule : RamshaModule
{

    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityCoreModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        var IdentityBuilder = context.Services.AddIdentityCore<RamshaIdentityUser>(options =>
     {
         options.Password.RequireDigit = false;
         options.Password.RequireNonAlphanumeric = false;
         options.Password.RequireUppercase = false;
         options.Password.RequiredLength = 4;
     })
         .AddRoles<RamshaIdentityRole>()
        .AddClaimsPrincipalFactory<RamshaUserClaimsPrincipalFactory>();

        context.Services.AddObjectAccessor(IdentityBuilder);

        context.Services.ExecutePreConfigured(IdentityBuilder);
    }
}
