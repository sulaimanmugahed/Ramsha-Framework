namespace Ramsha.EntityFrameworkCore;

public interface IRamshaDbContextConfigurator
{
    void Configure(RamshaDbContextConfigurationContext context);
}

public interface IRamshaDbContextConfigurator<TDbContext>
    where TDbContext : RamshaEFDbContext<TDbContext>
{
    void Configure(RamshaDbContextConfigurationContext<TDbContext> context);
}
