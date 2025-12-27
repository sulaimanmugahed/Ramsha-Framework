using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct WrongContextError(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    string Code = WRONG_CONTEXT,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<WrongContextError>
{
    /// <summary>The default status code for wrong context errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.WrongContext;
}
