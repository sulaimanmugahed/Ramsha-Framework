using System.Text.Json.Serialization;


namespace Ramsha;

/// <summary>
/// Represents a 201 Created result containing a new resource identifier and optional URL.
/// </summary>
public readonly record struct CreatedResult<TId>(TId Id, string? Url = null)
    : IRamshaSuccessResult<CreatedResult<TId>>
    where TId : IEquatable<TId>, IComparable<TId>
{
    /// <summary>The default status code for created results.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.Created;
}
