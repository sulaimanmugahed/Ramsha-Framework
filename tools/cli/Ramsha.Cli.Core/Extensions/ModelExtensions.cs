namespace Ramsha.Cli.Core;

public static class ModelExtensions
{
    public static T? Get<T>(this IHasParameter model, string key)
    {
        return model[key] is T value ? value : default;
    }
}
