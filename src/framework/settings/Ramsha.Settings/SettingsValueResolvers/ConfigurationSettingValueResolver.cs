using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Ramsha.Settings;

public class ConfigurationSettingValueResolver(IConfiguration configuration) : ISettingValueResolver
{
    public Task<T?> GetAsync<T>(SettingDefinition def)
    {
        var section = def.GroupName is not null ? configuration.GetSection(def.GroupName).GetSection(def.Name) : configuration.GetSection(def.Name);
        return Task.FromResult(section.Get<T>());
    }

    public string GetProviderName()
    {
        return "C";
    }
}
