using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public record UnauthenticatedError(
    string Code = UNAUTHENTICATED,
    string? Message = null,
    IEnumerable<NamedError>? Errors = null,
    RamshaErrorContext? Context = null
) : RamshaErrorResult(
    RamshaResultStatus.NotFound,
    Code,
    Message,
    Errors,
    Context
)
{
    public static implicit operator Task<IRamshaResult>(UnauthenticatedError error)
     => Task.FromResult<IRamshaResult>(error);
}
