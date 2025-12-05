using System.Text.Json;

namespace Ramsha.Settings;

public abstract class SettingValueResolver : ISettingValueResolver
{
    public abstract string GetProviderName();
    public abstract Task<T?> GetAsync<T>(SettingDefinition def);


    protected virtual T? Deserialize<T>(string raw, Type originalValueType)
    {
        var type = typeof(T);

        if (originalValueType != type)
        {
            throw new Exception("invalid setting type");
        }

        if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal))
            return (T)Convert.ChangeType(raw, type);

        return JsonSerializer.Deserialize<T>(raw);
    }

}
