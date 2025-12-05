namespace Ramsha.Settings;

public interface ISettingStore
{
    Task<string?> GetValueAsync(string name);
}
