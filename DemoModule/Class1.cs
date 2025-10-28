using Microsoft.Extensions.DependencyInjection;
using Ramsha;
using Ramsha.AspNetCore;

namespace DemoModule;

public class DemoModuleModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder context)
    {
        base.OnCreating(context);
        context.DependsOn<AspNetCoreModule>();
    }
    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
    }
}
