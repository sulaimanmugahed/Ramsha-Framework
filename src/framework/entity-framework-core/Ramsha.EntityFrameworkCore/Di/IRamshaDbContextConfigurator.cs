namespace Ramsha.EntityFrameworkCore;

public interface IRamshaDbContextConfigurator
{
    void Configure(RamshaDbContextConfigurationContext context);
}
