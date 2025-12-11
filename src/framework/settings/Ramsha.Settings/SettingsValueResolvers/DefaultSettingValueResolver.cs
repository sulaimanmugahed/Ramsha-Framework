

namespace Ramsha.Settings;

public class MemorySettingValueResolver : SettingValueResolver
{
    public override Task<T?> ResolveAsync<T>(SettingDefinition def) where T : default
    {
        return Task.FromResult(def.DefaultValue is not null ? (T)def.DefaultValue : default);
    }

    public override string GetProviderName()
    {
        return "D";
    }
}
