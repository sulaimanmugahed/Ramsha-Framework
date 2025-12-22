using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Ramsha.Settings;

public class ConfigurationSettingValueResolver(IConfiguration configuration)
: SettingValueResolver("C")
{
    public override Task<T?> ResolveAsync<T>(SettingDefinition def)
     where T : default
    {
        var section = def.GroupName is not null ? configuration.GetSection(def.GroupName).GetSection(def.Name) : configuration.GetSection(def.Name);
        return Task.FromResult(section.Get<T>());
    }
}
