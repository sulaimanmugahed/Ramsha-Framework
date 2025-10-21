namespace Ramsha;

[Serializable]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectableAttribute : Attribute
{
    public object? ServiceKey { get; init; }

    public override bool Equals(object? obj) => Match(obj);

    public override bool Match(object? obj)
    {
        if (obj is InjectableAttribute attribute)
        {
            return object.Equals(ServiceKey, attribute.ServiceKey);
        }
        return false;
    }

    public override int GetHashCode() => ServiceKey?.GetHashCode() ?? 0;

    public override bool IsDefaultAttribute() => ServiceKey == null;
}
