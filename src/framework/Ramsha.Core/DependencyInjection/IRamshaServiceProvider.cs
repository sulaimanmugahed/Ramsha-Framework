using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;



namespace Ramsha;

public interface IRamshaServiceProvider
{
    T? GetService<T>();
    T GetRequiredService<T>();
    T? GetKeyedService<T>(object? key);
    T GetRequiredKeyedService<T>(object? key);
}

public class RamshaServiceProvider : IRamshaServiceProvider
{
    private readonly IServiceProvider _sp;
    private readonly ConcurrentDictionary<ServiceId, Lazy<object?>> _cache = new();

    public RamshaServiceProvider(IServiceProvider sp)
    {
        _sp = sp;
        _cache.TryAdd(new ServiceId(null, typeof(IServiceProvider)),
            new Lazy<object?>(() => _sp));
    }

    public T? GetService<T>() => (T?)GetService(typeof(T));

    public object? GetService(Type type)
        => _cache.GetOrAdd(new ServiceId(null, type),
            _ => new Lazy<object?>(() => _sp.GetService(type))).Value;

    public T GetRequiredService<T>() => (T)GetRequiredService(typeof(T));

    public object GetRequiredService(Type type)
        => _cache.GetOrAdd(new ServiceId(null, type),
            _ => new Lazy<object?>(() => _sp.GetRequiredService(type))).Value!;


    public T? GetKeyedService<T>(object? key)
        => (T?)_cache.GetOrAdd(new ServiceId(key, typeof(T)),
            _ => new Lazy<object?>(() => _sp.GetKeyedServices<T>(key).FirstOrDefault())).Value;

    public T GetRequiredKeyedService<T>(object? key)
        => (T)GetRequiredKeyedService(typeof(T), key);

    public object GetRequiredKeyedService(Type type, object? key)
        => _cache.GetOrAdd(new ServiceId(key, type),
            _ => new Lazy<object?>(() => _sp.GetRequiredKeyedService(type, key))).Value!;
}

public readonly struct ServiceId : IEquatable<ServiceId>
{
    public object? Key { get; }
    public Type ServiceType { get; }

    public ServiceId(object? key, Type serviceType)
    {
        Key = key;
        ServiceType = serviceType;
    }

    public bool Equals(ServiceId other)
        => Equals(Key, other.Key) && ServiceType == other.ServiceType;

    public override bool Equals(object? obj)
        => obj is ServiceId other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Key, ServiceType);

    public override string ToString()
        => $"{ServiceType.FullName} ({Key ?? "no-key"})";
}

