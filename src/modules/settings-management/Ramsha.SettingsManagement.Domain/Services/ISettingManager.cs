using Ramsha.Settings;
using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Domain;

public interface ISettingManager
{
    Task<T?> GetValueAsync<T>(string settingName, string providerName, string? providerKey);
    Task<object?> GetValueAsync(string settingName, string providerName, string? providerKey);

    Task<List<SettingValue>> GetAllAsync(string providerName, string? providerKey);

    Task SetAsync<T>(string settingName, T? settingValue, string providerName, string? providerKey, bool replaceExist = true);


    Task SetAsync(string settingName, string? jsonSettingValue, string providerName, string? providerKey, bool replaceExist = true);

    Task DeleteAsync(string settingName, string providerName, string? providerKey);
}
