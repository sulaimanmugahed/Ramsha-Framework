namespace Ramsha.Settings;

public interface ISettingResolver
{
    Task<T?> GetAsync<T>(string name);
}
