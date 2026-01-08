using System.Reflection;
using Ramsha;


namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static T CreateInstanceWithPropInjection<T>(this IServiceProvider provider)
    {
        var instance = ActivatorUtilities.CreateInstance<T>(provider);

        foreach (var prop in GetInjectableProperties(typeof(T)))
        {
            var attribute = prop.GetCustomAttribute<InjectableAttribute>();
            object? value = attribute?.ServiceKey != null
                ? provider.GetKeyedService(prop.PropertyType, attribute.ServiceKey)
                : provider.GetService(prop.PropertyType);

            if (value != null)
                prop.SetValue(instance, value);
        }

        return instance;
    }


    public static object CreateInstanceWithPropInjection(this IServiceProvider provider, Type type)
    {
        var instance = ActivatorUtilities.CreateInstance(provider, type);

        foreach (var prop in GetInjectableProperties(type))
        {
            var attribute = prop.GetCustomAttribute<InjectableAttribute>();
            object? value = attribute?.ServiceKey != null
                ? provider.GetKeyedService(prop.PropertyType, attribute.ServiceKey)
                : provider.GetService(prop.PropertyType);

            if (value != null)
                prop.SetValue(instance, value);
        }

        return instance;
    }

    private static IEnumerable<PropertyInfo> GetInjectableProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite &&
                        p.IsDefined(typeof(InjectableAttribute), true));
    }

    public static Lazy<T> GetLazyService<T>(this IServiceProvider serviceProvider) where T : class
    {
        return new GenericLazy<T>(() => serviceProvider.GetService<T>()!);
    }

    public static Lazy<T> GetLazyKeyedService<T>(this IServiceProvider serviceProvider, object serviceKey) where T : class
    {
        return new GenericLazy<T>(() => serviceProvider.GetKeyedService<T>(serviceKey)!);
    }

    public static Lazy<T> GetLazyRequiredService<T>(this IServiceProvider serviceProvider) where T : class
    {
        return new GenericLazy<T>(() => serviceProvider.GetRequiredService<T>());
    }

    public static Lazy<T> GetLazyRequiredKeyedService<T>(this IServiceProvider serviceProvider, object serviceKey) where T : class
    {
        return new GenericLazy<T>(() => serviceProvider.GetRequiredKeyedService<T>(serviceKey));
    }
}
