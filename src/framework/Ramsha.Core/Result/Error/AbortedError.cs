
using System.Text.Json.Serialization;
using static Ramsha.RamshaErrorsCodes;

namespace Ramsha;

public readonly record struct AbortedError(
    string Code = ABORTED,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<AbortedError>
{
    /// <summary>The default status code for aborted operations.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.Aborted;

    private static AbortedError _Self = new(ABORTED);

    /// <summary>Cached instance for common aborted errors.</summary>
    public static ref AbortedError Value => ref _Self;
}
