using MyApp.Api;
using MyApp.Application;
using Ramsha;

namespace MyApp.Startup;

public class MyAppStartupModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<MyAppApplicationModule>()
        .DependsOn<MyAppApiModule>();
    }
}
