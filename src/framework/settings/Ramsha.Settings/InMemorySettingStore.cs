namespace Ramsha.Settings;

public class NoneSettingStore : ISettingStore
{
    public Task<T?> GetValueAsync<T>(SettingDefinition def, string providerName, string providerKey)
    {
        return Task.FromResult<T?>(default);
    }

    public Task<object?> GetValueAsync(SettingDefinition def, string providerName, string providerKey)
    {
        return Task.FromResult<object?>(null);
    }
}
