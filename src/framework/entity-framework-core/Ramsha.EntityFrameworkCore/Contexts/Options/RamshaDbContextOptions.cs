using Microsoft.EntityFrameworkCore;

namespace Ramsha.EntityFrameworkCore;

public class RamshaDbContextOptions
{
    internal List<Action<RamshaDbContextConfigurationContext>> DefaultPreConfigureActions { get; }

    internal Action<RamshaDbContextConfigurationContext>? DefaultConfigureAction { get; set; }

    internal Dictionary<Type, List<object>> PreConfigureActions { get; }

    internal Dictionary<Type, object> ConfigureActions { get; }

    internal Dictionary<Type, Type> DbContextReplacements { get; }

    internal Action<DbContext, ModelConfigurationBuilder>? DefaultConventionAction { get; set; }

    internal Dictionary<Type, List<object>> ConventionActions { get; }

    internal Action<DbContext, ModelBuilder>? DefaultOnModelCreatingAction { get; set; }

    internal Dictionary<Type, List<object>> OnModelCreatingActions { get; }

    public RamshaDbContextOptions()
    {
        DefaultPreConfigureActions = [];
        PreConfigureActions = [];
        ConfigureActions = [];
        DbContextReplacements = [];
        ConventionActions = [];
        OnModelCreatingActions = [];
    }

    public void PreConfigure(Action<RamshaDbContextConfigurationContext> action)
    {

        DefaultPreConfigureActions.Add(action);
    }

    public void Configure(Action<RamshaDbContextConfigurationContext> action)
    {

        DefaultConfigureAction = action;
    }

    public void ConfigureDefaultConvention(Action<DbContext, ModelConfigurationBuilder> action)
    {
        DefaultConventionAction = action;
    }

    public void ConfigureConventions<TDbContext>(Action<TDbContext, ModelConfigurationBuilder> action)
        where TDbContext : RamshaDbContext<TDbContext>
    {

        var actions = ConventionActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value;
        if (actions == null)
        {
            ConventionActions[typeof(TDbContext)] = new List<object>
            {
                new Action<DbContext, ModelConfigurationBuilder>((dbContext, builder) => action((TDbContext)dbContext, builder))
            };
            return;
        }

        actions.Add(action);
    }

    public void ConfigureDefaultOnModelCreating(Action<DbContext, ModelBuilder> action)
    {

        DefaultOnModelCreatingAction = action;
    }

    public void ConfigureOnModelCreating<TDbContext>(Action<TDbContext, ModelBuilder> action)
        where TDbContext : RamshaDbContext<TDbContext>
    {
        var actions = OnModelCreatingActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value;
        if (actions == null)
        {
            OnModelCreatingActions[typeof(TDbContext)] = new List<object>
            {
                new Action<DbContext, ModelBuilder>((dbContext, builder) => action((TDbContext)dbContext, builder))
            };
            return;
        }

        actions.Add(action);
    }

    public bool IsConfiguredDefault()
    {
        return DefaultConfigureAction != null;
    }

    public void PreConfigure<TDbContext>(Action<RamshaDbContextConfigurationContext<TDbContext>> action)
        where TDbContext : RamshaDbContext<TDbContext>
    {

        var actions = PreConfigureActions.FirstOrDefault(x => x.Key == typeof(TDbContext)).Value;
        if (actions == null)
        {
            PreConfigureActions[typeof(TDbContext)] = actions = new List<object>();
        }

        actions.Add(action);
    }

    public void Configure<TDbContext>(Action<RamshaDbContextConfigurationContext<TDbContext>> action)
        where TDbContext : RamshaDbContext<TDbContext>
    {
        ConfigureActions[typeof(TDbContext)] = action;
    }

    public bool IsConfigured<TDbContext>()
    {
        return IsConfigured(typeof(TDbContext));
    }

    public bool IsConfigured(Type dbContextType)
    {
        return ConfigureActions.ContainsKey(dbContextType);
    }

    internal Type GetReplacedTypeOrSelf(Type dbContextType)
    {
        var replacementType = dbContextType;
        while (true)
        {
            var foundType = DbContextReplacements.LastOrDefault(x => x.Key == replacementType);
            if (!foundType.Equals(default(KeyValuePair<Type, Type>)))
            {
                if (foundType.Value == dbContextType)
                {
                    throw new Exception(
                        "Circular DbContext replacement found for " +
                        dbContextType.AssemblyQualifiedName
                    );
                }
                replacementType = foundType.Value;
            }
            else
            {
                return replacementType;
            }
        }
    }
}

