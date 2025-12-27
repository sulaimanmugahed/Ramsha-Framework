using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct InvalidError(
    string Code = INVALID,
    string? Message = null,
    IEnumerable<NamedError>? Errors = null,
    RamshaErrorContext? Context = null
) : IRamshaErrorResult<InvalidError>
{
    public static ResultStatus DefaultStatus => ResultStatus.Invalid;
    private static InvalidError _Self = new(INVALID);
    public static ref InvalidError Value => ref _Self;
}
