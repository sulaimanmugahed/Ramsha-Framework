using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Application;
using Ramsha.Settings;
using Ramsha.SettingsManagement.Contracts;
using Ramsha.SettingsManagement.Domain;
using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Application;

public class SettingsService(ISettingManager manager) : RamshaService, ISettingsService
{
    public Task DeleteAsync(string settingName, string providerName, string? providerKey)
    {
        return manager.DeleteAsync(settingName, providerName, providerKey);
    }

    public async Task<List<SettingValueDto>> GetAllAsync(string providerName, string? providerKey)
    {
        var settings = await manager.GetAllAsync(providerName, providerKey);

        return settings.Select(x => new SettingValueDto(x.Name, x.Value)).ToList();
    }

    public Task<object?> GetValueAsync(string settingName, string providerName, string? providerKey)
    {
        return manager.GetValueAsync(settingName, providerName, providerKey);
    }

    public Task SetAsync(string settingName, SetSettingInfo dto)
    {
        return manager.SetAsync(settingName, dto.SettingValue.HasValue ? dto.SettingValue.Value.GetRawText() : null, dto.ProviderName, dto.ProviderKey);
    }
}
