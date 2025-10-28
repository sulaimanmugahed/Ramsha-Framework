namespace Ramsha.EntityFrameworkCore;

public class RamshaDbContextConfiguratorAction : IRamshaDbContextConfigurator
{
    public Action<RamshaDbContextConfigurationContext> Action { get; }

    public RamshaDbContextConfiguratorAction(Action<RamshaDbContextConfigurationContext> action)
    {
        Action = action;
    }

    public void Configure(RamshaDbContextConfigurationContext context)
    {
        Action.Invoke(context);
    }
}

public class RamshaDbContextConfiguratorAction<TDbContext> : RamshaDbContextConfiguratorAction
    where TDbContext : RamshaDbContext<TDbContext>
{
    public RamshaDbContextConfiguratorAction(Action<RamshaDbContextConfigurationContext> action)
        : base(action)
    {
    }
}
