using Microsoft.Extensions.DependencyInjection;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.UnitOfWork;

public class UnitOfWorkModule : RamshaModule
{
    override public void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        context.Services.AddSingleton<ICurrentUnitOfWork, CurrentUnitOfWork>();
        context.Services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();
        context.Services.AddSingleton<IUnitOfWorkOptions, UnitOfWorkOptions>();


    }
}
