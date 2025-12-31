using static Ramsha.RamshaErrorsCodes;


namespace Ramsha;

public record InternalError(
    string Code = INTERNAL_ERROR,
    string? Message = null,
    IEnumerable<NamedError>? Errors = null,
    RamshaErrorContext? Context = null
) : RamshaErrorResult(
    RamshaResultStatus.InternalError,
    Code,
    Message,
    Errors,
    Context
    )
{
    public static implicit operator Task<IRamshaResult>(InternalError error)
   => Task.FromResult<IRamshaResult>(error);
}