using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct TooManyRequestsError(
    string Code = TOO_MANY_REQUESTS,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<TooManyRequestsError>
{
    /// <summary>The default status code for too many requests errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.TooManyRequests;

    private static TooManyRequestsError _Self = new(TOO_MANY_REQUESTS);

    /// <summary>Cached instance for common too many requests errors.</summary>
    public static ref TooManyRequestsError Value => ref _Self;
}
