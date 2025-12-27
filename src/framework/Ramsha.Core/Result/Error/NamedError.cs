
using System.Text.Json.Serialization;

namespace Ramsha;

/// <summary>
/// Represents an error associated with a specific name or key (e.g., a property or field).
/// </summary>
/// <param name="Key">The key or field name that the error relates to.</param>
/// <param name="Details">Zero or more detailed error entries for the key.</param>
/// <param name="Data">Optional extra data related to the error.</param>
public record NamedError(
    string Key,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedErrorDetails>? Details = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] object? Data = null
);

/// <summary>
/// Provides detailed information about a key-specific error.
/// </summary>
/// <param name="Message">A human-readable error message for the key error.</param>
/// <param name="Code">A machine-readable error code for the key error.</param>
/// <param name="Severity">Severity of the error detail.</param>
/// <param name="Meta">Optional extensibility payload.</param>
public record NamedErrorDetails(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message,
    string? Code = null,
    ErrorSeverity Severity = ErrorSeverity.Error,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] object? Meta = null
);

/// <summary>
/// Severity classification for error details.
/// </summary>
public enum ErrorSeverity
{
    Error,
    Warning,
    Information,
}
