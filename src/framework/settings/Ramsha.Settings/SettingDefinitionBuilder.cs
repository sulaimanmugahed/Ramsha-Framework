namespace Ramsha.Settings;

public class SettingDefinitionBuilder
{
    private readonly SettingDefinition _permission;

    public SettingDefinitionBuilder(SettingDefinition permission)
    {
        _permission = permission;
    }

    public SettingDefinitionBuilder Description(string description)
    {
        _permission.Description = description;
        return this;
    }

    public SettingDefinitionBuilder Providers(string[] providers)
    {
        _permission.Providers = providers.ToList();
        return this;
    }

}
