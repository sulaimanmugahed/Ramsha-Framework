using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoModule;
using Ramsha;
using Ramsha.AspNetCore;

namespace DemoApp;

public class AppModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder moduleBuilder)
    {
        base.OnModuleCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<DemoModuleModule>();
    }

    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);

    }

    public override void OnAppInit(InitContext context)
    {

        base.OnAppInit(context);
    }

    public override void OnAppShutdown(ShutdownContext context)
    {
        context.ServiceProvider.GetRequiredService<ILogger<AppModule>>().LogError("application shutdown");
        base.OnAppShutdown(context);
    }
}
