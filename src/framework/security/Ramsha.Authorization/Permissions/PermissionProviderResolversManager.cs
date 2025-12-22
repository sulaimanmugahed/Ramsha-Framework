using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Ramsha.Authorization;

public class PermissionProviderResolversManager
{
    public IReadOnlyList<IPermissionValueResolver> PermissionResolvers => _lazyResolvers.Value;
    private readonly Lazy<List<IPermissionValueResolver>> _lazyResolvers;

    public PermissionProviderResolversManager(
        IServiceProvider serviceProvider,
        IOptions<RamshaPermissionOptions> options)
    {
        Options = options.Value;
        ServiceProvider = serviceProvider;
        _lazyResolvers = new(GetResolvers, true);
    }

    protected RamshaPermissionOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    protected virtual List<IPermissionValueResolver> GetResolvers()
    {
        var providers = Options
            .ValueResolvers
            .Select(type => (ServiceProvider.GetRequiredService(type) as IPermissionValueResolver)!)
            .ToList();

        var multipleResolvers = providers.GroupBy(p => p.GetProviderName()).FirstOrDefault(x => x.Count() > 1);
        if (multipleResolvers != null)
        {
            throw new Exception($"Duplicate permission value provider name detected");
        }

        return providers;
    }
}


