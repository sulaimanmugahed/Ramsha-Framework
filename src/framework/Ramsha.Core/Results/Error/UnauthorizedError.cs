using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public record ForbiddenError(
    string Code = FORBIDDEN,
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
    public static implicit operator Task<IRamshaResult>(ForbiddenError error)
     => Task.FromResult<IRamshaResult>(error);
}
