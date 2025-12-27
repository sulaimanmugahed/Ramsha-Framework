using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct TimeoutError(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    string Code = TIMEOUT,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<TimeoutError>
{
    /// <summary>The default status code for timeout errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.CriticalTimeout;

    private static TimeoutError _Self = new();

    /// <summary>Cached instance for common timeout errors.</summary>
    public static ref TimeoutError Value => ref _Self;
}
