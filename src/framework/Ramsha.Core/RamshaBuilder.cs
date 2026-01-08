

namespace Ramsha;

public class RamshaBuilder
{
    internal List<Type> Modules { get; } = new();
    internal List<Action<PrepareContext>> PrepareActions { get; } = new();

    public RamshaBuilder AddModule<T>() where T : IRamshaModule
    {
        var moduleType = typeof(T);
        if (!Modules.Any(x => x == moduleType))
            Modules.Add(moduleType);
        return this;
    }

    public RamshaBuilder PrepareOptions<TOptions>(Action<TOptions> optionsAction)
      where TOptions : class
    {
        PrepareActions.Add(ctx => ctx.PrepareOptions(optionsAction));
        return this;
    }
}
