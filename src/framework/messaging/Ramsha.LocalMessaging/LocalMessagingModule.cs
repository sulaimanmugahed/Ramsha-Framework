
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.LocalMessaging;

public class LocalMessagingModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder moduleBuilder)
    {
        base.OnModuleCreating(moduleBuilder);
        moduleBuilder.DependsOn<LocalMessagingAbstractionsModule>();
    }

    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);


    }
}