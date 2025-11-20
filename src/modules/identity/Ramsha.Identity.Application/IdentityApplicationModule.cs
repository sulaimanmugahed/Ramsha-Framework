using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Application.Extensions;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;
using Ramsha.Identity.Domain;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Application;

public class IdentityApplicationModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder
        .DependsOn<IdentityContractsModule>()
        .DependsOn<IdentityDomainModule>();


        moduleBuilder.OnCreatingConfigure<LocalMessagingOptions>(messageOptions =>
        {
            messageOptions.AddCommandHandler(typeof(CreateRoleCommandHandler<,,>));
        });
        ;





    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddRamshaIdentityApplicationServices();
    }
}
