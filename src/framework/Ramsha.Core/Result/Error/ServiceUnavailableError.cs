using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct ServiceUnavailableError(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    string Code = SERVICE_UNAVAILABLE,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<ServiceUnavailableError>
{
    /// <summary>The default status code for service unavailable errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.CriticalServiceUnavailable;
}
