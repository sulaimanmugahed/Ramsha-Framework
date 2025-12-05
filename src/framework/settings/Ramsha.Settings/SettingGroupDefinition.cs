namespace Ramsha.Settings;

public class SettingGroupDefinition
{
    public string Name { get; set; }
    public List<SettingDefinition> SettingDefinitions { get; set; } = [];

    public SettingGroupDefinition(string name)
    {
        Name = name;
    }

    public static SettingGroupDefinition Create(string name)
        => new SettingGroupDefinition(name);

}
