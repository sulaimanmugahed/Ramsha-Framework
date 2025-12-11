using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace Ramsha.SettingsManagement.Domain;

public interface ISettingManagerResolver
{
    string GetProviderName();
    Task<T?> GetValueAsync<T>(SettingDefinition setting, string providerKey);
    Task<object?> GetValueAsync(SettingDefinition setting, string providerKey);
    Task SetAsync<T>(SettingDefinition setting, T? settingValue, string providerKey);
    Task SetAsync(SettingDefinition setting, string? jsonSettingValue, string providerKey);
}
