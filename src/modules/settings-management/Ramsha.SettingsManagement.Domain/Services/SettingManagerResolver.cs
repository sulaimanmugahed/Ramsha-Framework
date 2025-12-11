using Ramsha.Settings;

namespace Ramsha.SettingsManagement.Domain;

public abstract class SettingManagerResolver(string providerName, ISettingManagerStore store) : ISettingManagerResolver
{

    public virtual string GetProviderName() => providerName;

    public virtual Task<T?> GetValueAsync<T>(SettingDefinition setting, string providerKey)
    {
        return store.GetValueAsync<T>(setting, GetProviderName(), GetNormalizedProviderKey(providerKey));
    }

    public virtual Task<object?> GetValueAsync(SettingDefinition setting, string providerKey)
    {
        return store.GetValueAsync(setting, GetProviderName(), GetNormalizedProviderKey(providerKey));
    }

    public virtual Task SetAsync<T>(SettingDefinition setting, T? settingValue, string providerKey)
    {
        return store.SetAsync(setting, settingValue, GetProviderName(), GetNormalizedProviderKey(providerKey));
    }

    public virtual Task SetAsync(SettingDefinition setting, string? jsonSettingValue, string providerKey)
    {
        return store.SetAsync(setting, jsonSettingValue, GetProviderName(), GetNormalizedProviderKey(providerKey));
    }

    protected virtual string GetNormalizedProviderKey(string providerKey)
    {
        return providerKey;
    }
}
