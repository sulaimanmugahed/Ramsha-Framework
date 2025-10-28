namespace Ramsha.Domain;

public static class ConnectionStringResolverExtensions
{
    public static Task<string?> ResolveAsync<T>(this IConnectionStringResolver services)
    {
        return services.ResolveAsync(ConnectionStringAttribute.GetNameOrNull<T>());
    }
    public static Task<string?> ResolveAsync(this IConnectionStringResolver services, Type type)
    {
        return services.ResolveAsync(ConnectionStringAttribute.GetNameOrNull(type));
    }
}
