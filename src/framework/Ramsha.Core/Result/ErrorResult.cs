

using System.Text.Json.Serialization;
using static Ramsha.RamshaErrorsCodes;

namespace Ramsha;



public record RamshaErrorResult(
    ResultStatus Status,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string Code = ERROR,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    RamshaErrorContext? Context = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<NamedError>? Errors = null
) : IRamshaErrorResult
;
