using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ramsha.Common.Domain;
using Ramsha.Settings;
using Ramsha.SettingsManagement.Domain;
using Ramsha.SettingsManagement.Shared;

namespace Ramsha.SettingsManagement.Domain;

public class SettingManager(
    ISettingDefinitionStore settingDefinition,
     ISettingManagerStore managerStore,
     IOptions<SettingsManagementOptions> options,
     IServiceProvider serviceProvider) : RamshaDomainManager, ISettingManager
{
    protected List<ISettingManagerResolver> Resolvers => _lazyResolvers.Value;
    private readonly Lazy<List<ISettingManagerResolver>> _lazyResolvers = new Lazy<List<ISettingManagerResolver>>(
            () => options
                .Value
                .ManagerResolvers
                .Select(c => serviceProvider.GetRequiredService(c) as ISettingManagerResolver)
                .ToList(),
            true
        );

    public async Task DeleteAsync(string settingName, string providerName, string? providerKey)
    {
        await managerStore.DeleteAsync(settingName, providerName, providerKey);
    }

    public async Task<List<SettingValue>> GetAllAsync(string providerName, string? providerKey)
    {
        var allDefinitions = await settingDefinition.GetAllAsync();
        return await managerStore.GetAllAsync(allDefinitions, providerName, providerKey);
    }

    public async Task<object?> GetValueAsync(string settingName, string providerName, string? providerKey)
    {
        var def = await settingDefinition.FindAsync(settingName);
        if (def is null)
        {
            return default;
        }

        var resolver = Resolvers.FirstOrDefault(c => c.GetProviderName() == providerName);
        object? value = default;


        if (resolver is null)
        {
            throw new Exception($"no provider with this name: {providerName}");
        }

        value = await resolver.GetValueAsync(def, providerKey);


        return value ?? def.DefaultValue;

    }

    public async Task<T?> GetValueAsync<T>(string settingName, string providerName, string? providerKey)
    {
        var def = await settingDefinition.FindAsync(settingName);
        if (def is null)
        {
            return default;
        }

        var resolver = Resolvers.FirstOrDefault(c => c.GetProviderName() == providerName);
        T? value = default;

        if (resolver is null)
        {
            throw new Exception($"no provider with this name: {providerName}");
        }
        value = await resolver.GetValueAsync<T>(def, providerKey);

        return value ?? (T?)def.DefaultValue;

    }

    public async Task SetAsync(string settingName, string? jsonSettingValue, string providerName, string? providerKey, bool replaceExist = true)
    {
        var def = await settingDefinition.FindAsync(settingName);
        if (def is null)
        {
            throw new Exception($"setting definition '{settingName}' not found");
        }

        var resolver = Resolvers.FirstOrDefault(c => c.GetProviderName() == providerName);
        if (resolver is null)
        {
            throw new Exception($"no provider with this name: {providerName}");
        }
        await resolver.SetAsync(def, jsonSettingValue, providerKey);

    }


    public async Task SetAsync<T>(string settingName, T? settingValue, string providerName, string? providerKey, bool replaceExist = true)
    {
        var def = await settingDefinition.FindAsync(settingName);
        if (def is null)
        {
            throw new Exception($"setting definition '{settingName}' not found");
        }
        var resolver = Resolvers.FirstOrDefault(c => c.GetProviderName() == providerName);
        if (resolver is null)
        {
            throw new Exception($"no provider with this name: {providerName}");
        }
        await resolver.SetAsync(def, settingValue, providerKey);
    }
}
