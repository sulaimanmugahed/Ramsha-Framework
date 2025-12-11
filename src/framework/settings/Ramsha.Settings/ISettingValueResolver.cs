namespace Ramsha.Settings;

public interface ISettingValueResolver
{
    string GetProviderName();
    Task<T?> ResolveAsync<T>(SettingDefinition def);
}
