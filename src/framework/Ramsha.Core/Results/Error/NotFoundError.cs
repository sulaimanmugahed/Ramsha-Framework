using static Ramsha.RamshaErrorsCodes;

namespace Ramsha;

public record NotFoundError(
        string Code = NOT_FOUND,
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

}