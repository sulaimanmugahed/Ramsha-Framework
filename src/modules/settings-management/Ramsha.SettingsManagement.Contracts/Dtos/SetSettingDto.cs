using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ramsha.SettingsManagement.Contracts;

public class SetSettingInfo
{
    public JsonElement? SettingValue { get; set; }
    public string ProviderName { get; set; }
    public string? ProviderKey { get; set; }
}
