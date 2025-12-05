using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Settings;

public class InMemorySettingDefinitionStore : ISettingDefinitionStore
{
    private readonly Lazy<Dictionary<string, SettingGroupDefinition>> _lazyGroupDefinitions;
    private IDictionary<string, SettingGroupDefinition> GroupDefinitions => _lazyGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, SettingDefinition>> _lazySettingDefinitions;
    private IDictionary<string, SettingDefinition> SettingDefinitions => _lazySettingDefinitions.Value;
    private readonly IServiceProvider _serviceProvider;
    private readonly RamshaSettingsOptions _options;

    public InMemorySettingDefinitionStore(IServiceProvider serviceProvider
    , IOptions<RamshaSettingsOptions> options)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _lazyGroupDefinitions = new(CreateGroups, true);
        _lazySettingDefinitions = new(CreateSettings, true);
    }

    private Dictionary<string, SettingGroupDefinition> CreateGroups()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = new SettingDefinitionContext();

        var providers = _options
        .DefinitionProviders
        .Select(dp => scope.ServiceProvider.GetRequiredService(dp) as ISettingDefinitionProvider)
        .ToList();

        foreach (var provider in providers)
            provider?.Define(context);

        return context.Groups;
    }


    private Dictionary<string, SettingDefinition> CreateSettings()
    {
        var settings = new Dictionary<string, SettingDefinition>();
        foreach (var group in GroupDefinitions.Values)
        {
            foreach (var setting in group.SettingDefinitions)
            {
                settings[setting.Name] = setting;
            }
        }


        return settings;
    }
    public async Task<SettingDefinition?> FindAsync(string name)
    {
        return SettingDefinitions.TryGetValue(name, out var def) ? def : null;
    }
}
