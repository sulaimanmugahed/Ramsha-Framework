using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Common.Domain;
using Ramsha.UnitOfWork;


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
        context.Services.AddTransient<SoftDeleteInterceptor>();

        context.Services.Configure<GlobalQueryFilterOptions>(options =>
        {
            options.DefaultStates[typeof(ISoftDelete)] = new GlobalQueryFilterState(true);
        });


        context.Services.Configure<RamshaDbContextOptions>(options =>
       {
           options.PreConfigure(configurationContext =>
           {
               configurationContext
               .DbContextOptions
               .AddInterceptors(
                configurationContext.ServiceProvider.GetRequiredService<EntityCreationInterceptor>(),
                configurationContext.ServiceProvider.GetRequiredService<EntityModificationInterceptor>(),
                configurationContext.ServiceProvider.GetRequiredService<SoftDeleteInterceptor>(),
                configurationContext.ServiceProvider.GetRequiredService<DomainEventToUnitOfWorkInterceptor>()
                    );
           });
       });

        context.Services.AddSingleton(typeof(EFGlobalQueryFilterApplier<>));
        context.Services.AddTransient<IEfDbContextTypeProvider, EfDbContextTypeProvider>();
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UoWDbContextProvider<>));

        context.Services.Configure<RamshaHooksOptions>(options =>
        {
            options.InitHookContributors.Add<EntityFrameworkCoreInitHookContributor>();
        });
    }
}
