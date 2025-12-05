using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Settings;

public class SettingValueResolverManager : ISettingValueResolverManager
{
    public IReadOnlyList<ISettingValueResolver> Resolvers => _lazyResolvers.Value;
    private readonly Lazy<List<ISettingValueResolver>> _lazyResolvers;

    public SettingValueResolverManager(
        IServiceProvider serviceProvider,
        IOptions<RamshaSettingsOptions> options)
    {
        Options = options.Value;
        ServiceProvider = serviceProvider;
        _lazyResolvers = new(GetResolvers, true);
    }

    protected RamshaSettingsOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    protected virtual List<ISettingValueResolver> GetResolvers()
    {
        var providers = Options
            .ValueResolvers
            .Select(type => (ServiceProvider.GetRequiredService(type) as ISettingValueResolver)!)
            .ToList();

        var multipleResolvers = providers.GroupBy(p => p.GetProviderName()).FirstOrDefault(x => x.Count() > 1);
        if (multipleResolvers != null)
        {
            throw new Exception($"Duplicate permission value provider name detected");
        }

        return providers;
    }

}
