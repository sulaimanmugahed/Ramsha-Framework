using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace Ramsha.SettingsManagement.Domain;

public class PersistSettingStore(ISettingManagerStore managerStore) : ISettingStore
{
    public Task<T?> GetValueAsync<T>(SettingDefinition def, string providerName, string providerKey)
    {
        return managerStore.GetValueAsync<T>(def, providerName, providerKey);
    }

    public async Task<object?> GetValueAsync(SettingDefinition def, string providerName, string providerKey)
    {
        return managerStore.GetValueAsync(def, providerName, providerKey);
    }
}
