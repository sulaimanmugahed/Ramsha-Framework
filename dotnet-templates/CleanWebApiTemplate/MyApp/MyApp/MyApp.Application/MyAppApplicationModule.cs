using MyApp.Contracts;
using MyApp.Domain;
using Ramsha;
using Ramsha.ApplicationAbstractions;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.LocalMessaging.Abstractions;

namespace MyApp.Application;

public class MyAppApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<MyAppContractsModule>()
        .DependsOn<MyAppDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }

    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);

        context.Configure<LocalMessagingOptions>(options =>
        {
            options.AddMessagesFromAssembly<MyAppApplicationModule>();
        });
    }
}