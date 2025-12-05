namespace Ramsha.Settings;

public class SettingDefinition
{
    public string Name { get; set; }
    public string? GroupName { get; set; }
    public Type ValueType { get; set; }
    public string? Description { get; set; }
    public object? DefaultValue { get; set; }
    public List<string> Providers { get; set; } = [];


    internal SettingDefinition(string name, Type valueType, object? defaultValue = null, string? groupName = null)
    {
        Name = name;
        ValueType = valueType;
        DefaultValue = defaultValue;
        GroupName = groupName;
    }

    public static SettingDefinition Create<T>(string name, T? defaultValue = default)
        => new SettingDefinition(name, typeof(T), defaultValue);

}
