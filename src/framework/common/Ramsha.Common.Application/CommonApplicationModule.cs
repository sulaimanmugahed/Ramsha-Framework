using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.ApplicationAbstractions;

public class CommonApplicationModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder
        .DependsOn<CommonDomainModule>()
        .DependsOn<LocalMessagingAbstractionsModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddCommonApplicationServices();
    }
}
