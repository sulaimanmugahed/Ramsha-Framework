using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct UnauthenticatedError(
    string Code = UNAUTHENTICATED,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<UnauthenticatedError>
{
    /// <summary>The default status code for unauthenticated errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.Unauthenticated;

    private static UnauthenticatedError _Self = new(UNAUTHENTICATED);

    /// <summary>Cached instance for common unauthenticated errors.</summary>
    public static ref UnauthenticatedError Value => ref _Self;
}
