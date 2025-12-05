namespace Ramsha.Settings;

public class SettingResolver(
    ISettingDefinitionStore definitionStore,
    ISettingValueResolverManager settingValueResolver) : ISettingResolver
{
    public async Task<T?> GetAsync<T>(string name)
    {
        var setting = await definitionStore.FindAsync(name);
        if (setting is null)
        {
            return default;
        }

        var resolvers = settingValueResolver.Resolvers;
        if (setting.Providers.Any())
        {
            resolvers = resolvers
            .Where(p => setting.Providers.Contains(p.GetProviderName()))
            .ToList();
        }

        foreach (var resolver in settingValueResolver.Resolvers)
        {
            if (ShouldSkipResolver(resolver.GetProviderName(), setting.Providers))
            {
                continue;
            }
            var value = await resolver.GetAsync<T>(setting);
            if (value != null)
            {
                return value;
            }
        }

        return default;
    }

    private bool ShouldSkipResolver(string resolverProvider, List<string> settingAllowedProviders)
    => settingAllowedProviders.Any() &&
                !settingAllowedProviders.Contains(resolverProvider);
}
