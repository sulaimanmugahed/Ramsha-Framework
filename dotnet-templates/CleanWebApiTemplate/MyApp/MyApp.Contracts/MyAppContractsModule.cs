using MyApp.Shared;
using Ramsha;

namespace MyApp.Contracts;

public class MyAppContractsModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<MyAppSharedModule>();
    }
}
