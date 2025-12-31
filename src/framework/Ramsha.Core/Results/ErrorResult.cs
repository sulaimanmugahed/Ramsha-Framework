

using System.Text.Json.Serialization;
using static Ramsha.RamshaErrorsCodes;

namespace Ramsha;



public record RamshaErrorResult(
    RamshaResultStatus Status,
     [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string Code = ERROR,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    RamshaErrorContext? Context = null
) : RamshaResult(Status), IRamshaErrorResult
{
    public static implicit operator Task<IRamshaResult>(RamshaErrorResult error)
    => Task.FromResult<IRamshaResult>(error);
    public override RamshaErrorResult? Error => this;

}

public record RamshaErrorResult<T>(
    RamshaResultStatus Status,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string Code = ERROR,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    RamshaErrorContext? Context = null
) : RamshaResult<T>(Status), IRamshaErrorResult
{
    public override RamshaErrorResult? Error => new(Status, Code, Message, Errors, Context);

}



