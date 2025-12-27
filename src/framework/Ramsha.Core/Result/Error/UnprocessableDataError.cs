using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct UnprocessableDataError(
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Message = null,
    string Code = UNPROCESSABLE_DATA,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    RamshaErrorContext? Context = null
) : IRamshaErrorResult<UnprocessableDataError>
{
    public static ResultStatus DefaultStatus => ResultStatus.UnprocessableData;
}
