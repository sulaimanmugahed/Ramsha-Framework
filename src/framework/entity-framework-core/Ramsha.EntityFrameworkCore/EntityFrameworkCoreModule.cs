using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Domain;
using Ramsha.UnitOfWork;

namespace Ramsha.EntityFrameworkCore;

public class EntityFrameworkCoreModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<DomainModule>()
        .DependsOn<UnitOfWorkModule>();
    }
    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        context.Services.Configure<RamshaDbContextOptions>(options =>
       {
           options.PreConfigure(dbContextConfigurationContext =>
           {
           });
       });

        context.Services.AddSingleton(typeof(EFGlobalQueryFilterApplier<>));

        context.Services.AddTransient<IEfDbContextTypeProvider, EfDbContextTypeProvider>();


        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UoWDbContextProvider<>));
    }
}
