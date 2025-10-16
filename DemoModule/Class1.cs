using Microsoft.Extensions.DependencyInjection;
using Ramsha;

namespace DemoModule;

public class DemoModuleModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder context)
    {
        base.OnModuleCreating(context);
    }
    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);
        context.Services.AddSingleton<ITestService, TestService>();

    }
}
