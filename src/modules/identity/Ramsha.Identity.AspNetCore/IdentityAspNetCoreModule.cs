using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.AspNetCore;

public class IdentityAspNetCoreModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityDomainModule>();

        moduleBuilder.PreConfigure<RamshaIdentityOptions>(options =>
        {
            options.ConfigureIdentity(builder =>
            {
                builder.AddDefaultTokenProviders()
              .AddSignInManager();
            });

        });

    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services
               .AddAuthentication(o =>
               {
                   o.DefaultScheme = IdentityConstants.ApplicationScheme;
                   o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
               })
               .AddIdentityCookies();
    }
}
