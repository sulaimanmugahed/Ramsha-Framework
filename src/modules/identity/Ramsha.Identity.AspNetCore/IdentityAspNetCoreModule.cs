using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.AspNetCore;

public class IdentityAspNetCoreModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<IdentityDomainModule>();
    }

    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);

        context.Configure<RamshaIdentityOptions>(options =>
        {
            options.ConfigureIdentity(builder =>
            {
                builder.AddDefaultTokenProviders()
              .AddSignInManager();
            });

        });

    }


    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services
               .AddAuthentication(o =>
               {
                   o.DefaultScheme = IdentityConstants.ApplicationScheme;
                   o.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
               }).AddIdentityCookies();

    }
}
