namespace Ramsha.Settings;

public interface ISettingDefinitionContext
{
    void Settings(params SettingDefinition[] definitions);
    void Setting(SettingDefinition definitions);
    SettingDefinitionBuilder Setting<T>(string name, T? defaultValue = default);
    SettingGroupDefinitionBuilder Group(string name, Action<SettingGroupDefinitionBuilder> configure);
    SettingDefinition? GetRootSetting(string name);
    SettingGroupDefinition? GetGroup(string name);

}
