using Microsoft.Extensions.DependencyInjection;
using Ramsha;

namespace DemoModule;

public class DemoModuleModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
    }
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
    }
}
