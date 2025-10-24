
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.LocalMessaging;

public class LocalMessagingModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<LocalMessagingAbstractionsModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);


    }
}