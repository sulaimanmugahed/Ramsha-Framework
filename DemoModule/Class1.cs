using Microsoft.Extensions.DependencyInjection;
using Ramsha;
using Ramsha.AspNetCore;

namespace DemoModule;

public class DemoModuleModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder context)
    {
        base.OnModuleCreating(context);
        context.DependsOn<AspNetCoreModule>();
    }
    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);
        context.Services.AddSingleton<ITestService, TestService>();

    }
}
