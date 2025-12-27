using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct NotFoundError(
    string Code = NOT_FOUND,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<NotFoundError>
{
    /// <summary>The default status code for not found errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.NotFound;

    private static NotFoundError _Self = new();

    /// <summary>Cached instance for common not found errors.</summary>
    public static ref NotFoundError Value => ref _Self;

    public static implicit operator Task<IRamshaResult>(NotFoundError error)
     => Task.FromResult<IRamshaResult>(error);
}
