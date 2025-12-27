using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct ForbiddenError(
    string Code = FORBIDDEN,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<ForbiddenError>
{
    /// <summary>The default status code for unauthorized errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.Forbidden;

    private static ForbiddenError _Self = new(FORBIDDEN);

    /// <summary>Cached instance for common unauthorized errors.</summary>
    public static ref ForbiddenError Value => ref _Self;
}
