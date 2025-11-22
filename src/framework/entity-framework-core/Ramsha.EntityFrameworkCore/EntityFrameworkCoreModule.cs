using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Common.Domain;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.EntityFrameworkCore;

public class EntityFrameworkCoreModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<CommonDomainModule>()
        .DependsOn<UnitOfWorkModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddTransient<DomainEventToUnitOfWorkInterceptor>();
        context.Services.AddTransient<EntityCreationInterceptor>();
        context.Services.AddTransient<EntityModificationInterceptor>();




        context.Services.Configure<RamshaDbContextOptions>(options =>
       {
           options.PreConfigure(configurationContext =>
           {
               configurationContext
               .DbContextOptions
               .AddInterceptors(
                configurationContext.ServiceProvider.GetRequiredService<EntityCreationInterceptor>(),
                configurationContext.ServiceProvider.GetRequiredService<EntityModificationInterceptor>(),
                configurationContext.ServiceProvider.GetRequiredService<DomainEventToUnitOfWorkInterceptor>()

                );
           });
       });

        context.Services.AddSingleton(typeof(EFGlobalQueryFilterApplier<>));

        context.Services.AddTransient<IEfDbContextTypeProvider, EfDbContextTypeProvider>();


        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UoWDbContextProvider<>));
    }


    public override async Task OnInitAsync(InitContext context)
    {
        base.OnInit(context);

        var provider = context.ServiceProvider;

        var uowManager = provider.GetRequiredService<IUnitOfWorkManager>();

        using var uow = uowManager.Begin(new UnitOfWorkOptions { IsTransactional = false });
        var dbProvider = provider.GetService<IDbContextProvider<IRamshaEFDbContext>>();
        if (dbProvider is not null)
        {
            var db = await dbProvider.GetDbContextAsync();
            await db.Database.EnsureCreatedAsync();
        }

        await uow.CompleteAsync();
    }
}
