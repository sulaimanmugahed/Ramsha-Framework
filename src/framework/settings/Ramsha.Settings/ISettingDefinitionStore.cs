namespace Ramsha.Settings;

public interface ISettingDefinitionStore
{
    Task<SettingDefinition?> FindAsync(string name);
    Task<IReadOnlyList<SettingDefinition>> GetAllAsync();

}
