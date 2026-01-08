namespace Ramsha;

public class RamshaHooksOptions
{
    public ITypeList<IInitHookContributor> InitHookContributors { get; }
    public ITypeList<IShutdownHookContributor> ShutdownHookContributors { get; }

    public RamshaHooksOptions()
    {
        InitHookContributors = new TypeList<IInitHookContributor>();
        ShutdownHookContributors = new TypeList<IShutdownHookContributor>();
    }
}
