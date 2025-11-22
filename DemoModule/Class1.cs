using Microsoft.Extensions.DependencyInjection;
using Ramsha;
using Ramsha.AspNetCore;

namespace DemoModule;

public class DemoModuleModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<AspNetCoreModule>();
    }
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
    }
}
