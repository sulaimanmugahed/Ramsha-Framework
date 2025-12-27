using static Ramsha.RamshaErrorsCodes;
    using System.Text.Json.Serialization;


    namespace Ramsha;

public readonly record struct ConflictError(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    string Code = CONFLICT,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<ConflictError>
{
    public static ResultStatus DefaultStatus => ResultStatus.Conflict;
}
