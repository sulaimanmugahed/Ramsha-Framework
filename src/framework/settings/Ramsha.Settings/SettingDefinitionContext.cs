namespace Ramsha.Settings;

public class SettingDefinitionContext : ISettingDefinitionContext
{
    private readonly Dictionary<string, SettingDefinition> _rootSettingDefinitions = new();
    private readonly Dictionary<string, SettingGroupDefinition> _groupDefinitions = new();


    public void Settings(params SettingDefinition[] definitions)
    {
        foreach (var definition in definitions)
        {
            _rootSettingDefinitions[definition.Name] = definition;
        }
    }


    public SettingGroupDefinitionBuilder Group(string name, Action<SettingGroupDefinitionBuilder> configure)
    {
        var group = SettingGroupDefinition.Create(name);

        var groupBuilder = new SettingGroupDefinitionBuilder(group);
        configure(groupBuilder);

        _groupDefinitions.Add(name, group);

        return groupBuilder;
    }



    public SettingGroupDefinition? GetGroup(string name)
   => _groupDefinitions.TryGetValue(name, out var def) ? def : null;

    public void Setting(SettingDefinition definition)
    {
        _rootSettingDefinitions[definition.Name] = definition;
    }

    public SettingDefinitionBuilder Setting<T>(string name, T? defaultValue = default)
    {
        var setting = SettingDefinition.Create(name, defaultValue);
        _rootSettingDefinitions[setting.Name] = setting;
        return new SettingDefinitionBuilder(setting);
    }

    public SettingDefinition? GetRootSetting(string name)
    => _rootSettingDefinitions.TryGetValue(name, out var def) ? def : null;


    public Dictionary<string, SettingDefinition> RootSetting => _rootSettingDefinitions;
    public Dictionary<string, SettingGroupDefinition> Groups => _groupDefinitions;
}
