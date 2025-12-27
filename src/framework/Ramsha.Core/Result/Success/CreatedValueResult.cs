using System.Text.Json.Serialization;


namespace Ramsha;

/// <summary>
/// Represents a 201 Created result containing a new resource identifier and the created value.
/// </summary>
public readonly record struct CreatedValueResult<TId, TValue>(TId Id, TValue Value, string? Url = null)
    : IRamshaValueSuccessResult<CreatedValueResult<TId, TValue>, TValue>
    where TId : IEquatable<TId>, IComparable<TId>
{
    /// <summary>The default status code for created value results.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.Created;

    /// <summary>Implicitly converts to <see cref="Created{TId}"/> by dropping the value.</summary>
    public static implicit operator CreatedResult<TId>(CreatedValueResult<TId, TValue> created) =>
        new(created.Id, created.Url);
}
