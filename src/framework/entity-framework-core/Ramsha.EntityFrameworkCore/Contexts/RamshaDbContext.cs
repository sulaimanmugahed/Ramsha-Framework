
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ramsha.Domain;

namespace Ramsha.EntityFrameworkCore;

public abstract class RamshaEFDbContext<TDbContext>(DbContextOptions<TDbContext> dbContextOptions)
: DbContext(dbContextOptions), IRamshaEFDbContext
where TDbContext : RamshaEFDbContext<TDbContext>
{
    public IServiceProvider ServiceProvider { get; set; } = default!;
    protected IOptions<RamshaDbContextOptions> Options => ServiceProvider.GetLazyRequiredService<IOptions<RamshaDbContextOptions>>().Value;
    protected IGlobalQueryFilterManager GlobalDataFilterManager => ServiceProvider.GetLazyRequiredService<IGlobalQueryFilterManager>().Value;

    public bool IsFilterEnabled<TFilter>()
    where TFilter : class
    => GlobalDataFilterManager.IsEnabled<TFilter>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        Options.Value.DefaultOnConfiguringAction?.Invoke(this, optionsBuilder);
        foreach (var onConfiguringAction in Options.Value.OnConfiguringActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value ?? [])
        {
            ((Action<DbContext, DbContextOptionsBuilder>)onConfiguringAction).Invoke(this, optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var filterApplier = ServiceProvider.GetRequiredService<EFGlobalQueryFilterApplier<TDbContext>>();
        filterApplier.ApplyFilters(modelBuilder, (TDbContext)this);

        Options.Value.DefaultOnModelCreatingAction?.Invoke(this, modelBuilder);
        foreach (var onModelCreatingAction in Options.Value.OnModelCreatingActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value ?? [])
        {
            ((Action<DbContext, ModelBuilder>)onModelCreatingAction).Invoke(this, modelBuilder);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        if (ServiceProvider == null || Options == null)
        {
            return;
        }

        Options.Value.DefaultConventionAction?.Invoke(this, configurationBuilder);
        foreach (var conventionAction in Options.Value.ConventionActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value ?? [])
        {
            ((Action<DbContext, ModelConfigurationBuilder>)conventionAction).Invoke(this, configurationBuilder);
        }
    }
}


