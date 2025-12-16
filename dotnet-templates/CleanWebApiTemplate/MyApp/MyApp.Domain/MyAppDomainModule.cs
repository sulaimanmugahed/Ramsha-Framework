using MyApp.Shared;
using Ramsha;
using Ramsha.Common.Domain;

namespace MyApp.Domain;

public class MyAppDomainModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<MyAppSharedModule>()
        .DependsOn<CommonDomainModule>();
    }
}
