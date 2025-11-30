namespace Ramsha.Authorization;

public class PermissionDefinitionManager
{
    private readonly IEnumerable<IPermissionDefinitionProvider> _providers;
    private readonly IPermissionDefinitionContext _context;

    public PermissionDefinitionManager(
        IEnumerable<IPermissionDefinitionProvider> providers,
        IPermissionDefinitionContext context)
    {
        _providers = providers;
        _context = context;
    }

    public void Initialize()
    {
        foreach (var provider in _providers)
            provider.Define(_context);
    }
}


