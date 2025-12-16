using MyApp.Contracts;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace MyApp.Api;

public class MyAppApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<MyAppContractsModule>()
        .DependsOn<AspNetCoreMvcModule>();
    }
}