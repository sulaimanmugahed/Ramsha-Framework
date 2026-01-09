using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Settings;
using Ramsha.SettingsManagement.Contracts;

namespace Ramsha.SettingsManagement.Api;

public class SettingsController(ISettingsService service) : RamshaApiController
{
    [HttpDelete("{settingName}")]
    public Task DeleteAsync(string settingName, string providerName, string? providerKey)
    {
        return service.DeleteAsync(settingName, providerName, providerKey);
    }

    [HttpGet]
    public Task<List<SettingValueDto>> GetAllAsync(string providerName, string? providerKey)
    {
        return service.GetAllAsync(providerName, providerKey);
    }

    [HttpGet("{settingName}")]
    public Task<object?> GetValueAsync(string settingName, string providerName, string? providerKey)
    {
        return service.GetValueAsync(settingName, providerName, providerKey);
    }

    [HttpPost("{settingName}")]
    public Task SetAsync(string settingName, SetSettingInfo dto)
    {
        return service.SetAsync(settingName, dto);
    }
}
