namespace Ramsha.Settings;

public class SettingGroupDefinitionBuilder
{
    private readonly SettingGroupDefinition _group;

    public SettingGroupDefinitionBuilder(SettingGroupDefinition group)
    {
        _group = group;
    }

    public void Setting(SettingDefinition definition)
    {
        definition.GroupName = _group.Name;
        _group.SettingDefinitions.Add(definition);
    }

    public void Settings(params SettingDefinition[] definitions)
    {
        _group.SettingDefinitions.AddRange(definitions.Select(x =>
        {
            x.GroupName = _group.Name;
            return x;
        }).ToList());
    }

    public SettingGroupDefinitionBuilder Providers(string[] providers)
    {
        _group.SettingDefinitions
       .Where(x => x.Providers is null)
       .Select(x =>
       {
           x.Providers = providers.ToList();
           return x;
       });

        return this;
    }

    public SettingDefinitionBuilder Setting<T>(string name, T? defaultValue = default)
    where T : class
    {
        var setting = new SettingDefinition(name, typeof(T), defaultValue, _group.Name);
        _group.SettingDefinitions.Add(setting);
        return new SettingDefinitionBuilder(setting);
    }
}
