using static Ramsha.RamshaErrorsCodes;


namespace Ramsha;

public record InvalidError(
    string Code = INVALID,
    string? Message = null,
    IEnumerable<NamedError>? Errors = null,
    RamshaErrorContext? Context = null
) : RamshaErrorResult(
    RamshaResultStatus.Invalid,
    Code,
    Message,
    Errors,
    Context
    )
{
    public static implicit operator Task<IRamshaResult>(InvalidError error)
   => Task.FromResult<IRamshaResult>(error);
}
