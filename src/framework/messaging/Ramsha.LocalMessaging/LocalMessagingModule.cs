

using Microsoft.Extensions.DependencyInjection;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.LocalMessaging;

public class LocalMessagingModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<LocalMessagingAbstractionsModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddScoped<IRamshaMediator, RamshaMediator>();
        context.Services.AddScoped<ILocalBus, LocalBus>();
        context.Services.AddScoped<IUnitOfWorkLocalEventBus, UnitOfWorkLocalBus>();

        var options = context.Services.ExecutePreparedOptions<LocalMessagingOptions>();

        context.Services.RegisterLocalMessagingOptions(options);
    }
}