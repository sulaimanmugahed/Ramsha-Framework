namespace Ramsha.Settings;

public interface ISettingStore
{
    Task<T?> GetValueAsync<T>(SettingDefinition def, string providerName, string providerKey);
    Task<object?> GetValueAsync(SettingDefinition def, string providerName, string providerKey);
}
