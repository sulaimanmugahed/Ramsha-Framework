using Microsoft.Extensions.DependencyInjection;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;

namespace Ramsha.Identity.Api;

public class IdentityApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
                .DependsOn<AspNetCoreMvcModule>()
        .DependsOn<IdentityContractsModule>();
    }
    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.PrepareOptions<IMvcBuilder>(builder =>
        {
            builder.AddIdentityGenericControllers();
        });
    }
}
