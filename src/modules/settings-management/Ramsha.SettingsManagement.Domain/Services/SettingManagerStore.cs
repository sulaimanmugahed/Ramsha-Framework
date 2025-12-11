using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace Ramsha.SettingsManagement.Domain;


public interface ISettingManagerStore
{
    Task<T?> GetValueAsync<T>(SettingDefinition setting, string providerName, string? providerKey);
    Task<object?> GetValueAsync(SettingDefinition setting, string providerName, string? providerKey);

    Task<List<SettingValue>> GetAllAsync(IEnumerable<SettingDefinition> definitions, string providerName, string? providerKey);

    Task SetAsync<T>(SettingDefinition setting, T? settingValue, string providerName, string? providerKey, bool replaceExist = true);

    Task SetAsync(SettingDefinition setting, string? jsonSettingValue, string providerName, string? providerKey, bool replaceExist = true);

    Task DeleteAsync(string settingName, string providerName, string? providerKey);
}


public class SettingManagerStore(ISettingRepository repository) : ISettingManagerStore
{
    public async Task DeleteAsync(string settingName, string providerName, string? providerKey)
    {
        var setting = await repository.FindAsync(x => x.Name == settingName && x.ProviderName == providerName && x.ProviderKey == providerKey);

        if (setting is not null)
        {
            await repository.DeleteAsync(setting);
        }
    }

    public async Task<List<SettingValue>> GetAllAsync(IEnumerable<SettingDefinition> definitions, string providerName, string? providerKey)
    {
        var allSettings = await repository.GetListAsync(
            x => x.ProviderName == providerName && x.ProviderKey == providerKey);

        var result = new List<SettingValue>();

        foreach (var def in definitions)
        {
            var existing = allSettings.FirstOrDefault(x => x.Name == def.Name);

            if (existing != null && existing.Value != null)
            {
                var value = JsonNode.Parse(existing.Value);
                if (value != null)
                    result.Add(new SettingValue(def.Name, value));
            }
            else if (def.DefaultValue is object tValue)
            {
                result.Add(new SettingValue(def.Name, tValue));
            }

        }

        return result;
    }

    public async Task<object?> GetValueAsync(SettingDefinition def, string providerName, string? providerKey)
    {
        var setting = await repository.FindAsync(x => x.Name == def.Name && x.ProviderName == providerName && x.ProviderKey == providerKey);
        if (setting is null || setting.Value is null)
        {
            return null;
        }

        return JsonNode.Parse(setting.Value);

    }

    public async Task<T?> GetValueAsync<T>(SettingDefinition def, string providerName, string? providerKey)
    {
        ValidateGenericSettingValue<T>(def);

        var setting = await repository.FindAsync(x => x.Name == def.Name && x.ProviderName == providerName && x.ProviderKey == providerKey);
        if (setting is null || setting.Value is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(setting.Value);

    }

    private static void ValidateGenericSettingValue<T>(SettingDefinition def)
    {
        if (def.ValueType != typeof(T))
        {
            throw new Exception("invalid setting type");
        }
    }

    private static void ValidateJsonSettingValue(string jsonSettingValue, SettingDefinition def)
    {
        try
        {
            JsonSerializer.Deserialize(jsonSettingValue, def.ValueType);
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Invalid JSON for setting '{def.Name}'. Expected type '{def.ValueType.Name}'. Error: {ex.Message}"
            );
        }
    }

    public async Task SetAsync(SettingDefinition def, string? jsonSettingValue, string providerName, string? providerKey, bool replaceExist = true)
    {

        if (jsonSettingValue is not null)
        {
            ValidateJsonSettingValue(jsonSettingValue, def);
        }

        var setting = await repository.FindAsync(
            x => x.Name == def.Name &&
                 x.ProviderName == providerName &&
                 x.ProviderKey == providerKey);

        if (setting is null)
        {
            setting = new Setting
            {
                Name = def.Name,
                ProviderName = providerName,
                ProviderKey = providerKey,
                Value = jsonSettingValue
            };

            await repository.AddAsync(setting);
        }
        else if (replaceExist)
        {
            setting.Value = RamshaJsonHelpers.MergeJson(setting.Value, jsonSettingValue);
        }
    }

    public async Task SetAsync<T>(SettingDefinition def, T? settingValue, string providerName, string? providerKey, bool replaceExist = true)
    {
        if (settingValue is not null)
        {
            ValidateGenericSettingValue<T>(def);
        }

        var setting = await repository.FindAsync(
            x => x.Name == def.Name &&
                 x.ProviderName == providerName &&
                 x.ProviderKey == providerKey);

        if (setting is null)
        {
            setting = new Setting
            {
                Name = def.Name,
                ProviderName = providerName,
                ProviderKey = providerKey,
                Value = JsonSerializer.Serialize(settingValue)
            };

            await repository.AddAsync(setting);
        }
        else if (replaceExist)
        {
            setting.Value = JsonSerializer.Serialize(settingValue);
        }
    }
}
