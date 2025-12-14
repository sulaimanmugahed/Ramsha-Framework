using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Application.Extensions;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;
using Ramsha.Identity.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.Common.Application;

namespace Ramsha.Identity.Application;

public class IdentityApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
              .DependsOn<IdentityContractsModule>()
        .DependsOn<IdentityDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }
    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.Configure<LocalMessagingOptions>(messageOptions =>
     {
         messageOptions.AddCommandHandler(typeof(CreateRoleCommandHandler<,,>));
     });
    }
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddRamshaIdentityApplicationServices();
    }
}
