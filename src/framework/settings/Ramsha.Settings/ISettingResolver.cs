namespace Ramsha.Settings;

public interface ISettingResolver
{
    Task<T?> ResolveAsync<T>(string name);
}
