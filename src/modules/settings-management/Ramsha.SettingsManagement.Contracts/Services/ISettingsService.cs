using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Contracts;

public interface ISettingsService
{
    Task<object?> GetValueAsync(string settingName, string providerName, string? providerKey);

    Task<List<SettingValueDto>> GetAllAsync(string providerName, string? providerKey);

    Task SetAsync(string settingName, SetSettingInfo dto);

    Task DeleteAsync(string settingName, string providerName, string? providerKey);
}
