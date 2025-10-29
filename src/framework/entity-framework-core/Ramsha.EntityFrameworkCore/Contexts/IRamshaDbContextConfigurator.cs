namespace Ramsha.EntityFrameworkCore;

public interface IRamshaDbContextConfigurator
{
    void Configure(RamshaDbContextConfigurationContext context);
}

public interface IRamshaDbContextConfigurator<TDbContext>
    where TDbContext : RamshaDbContext<TDbContext>
{
    void Configure(RamshaDbContextConfigurationContext<TDbContext> context);
}
