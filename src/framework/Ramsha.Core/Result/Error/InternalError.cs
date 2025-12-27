using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct InternalError(
    string Code = INTERNAL_ERROR,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<InternalError>
{
    /// <summary>The default status code for internal server errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.InternalError;
    private static InternalError _Self = new(INTERNAL_ERROR);
    public static ref InternalError Value => ref _Self;
}
