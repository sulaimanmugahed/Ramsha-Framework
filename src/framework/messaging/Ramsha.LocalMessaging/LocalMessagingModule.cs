

using Microsoft.Extensions.DependencyInjection;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.LocalMessaging;

public class LocalMessagingModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<LocalMessagingAbstractionsModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddScoped<IRamshaMediator, RamshaMediator>();
        context.Services.AddScoped<ILocalBus, LocalBus>();
        context.Services.AddScoped<IUnitOfWorkLocalEventBus, UnitOfWorkLocalBus>();

        var options = context.Services.ExecutePreConfigured<LocalMessagingOptions>();

        context.Services.RegisterLocalMessagingOptions(options);
    }
}