namespace Ramsha.Settings;

public interface ISettingValueResolver
{
    string GetProviderName();
    Task<T?> GetAsync<T>(SettingDefinition def);
}
