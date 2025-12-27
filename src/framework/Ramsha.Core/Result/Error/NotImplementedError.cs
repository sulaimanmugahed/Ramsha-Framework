using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct NotImplementedError(
    string Code = NOT_IMPLEMENTED,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<NotImplementedError>
{
    /// <summary>The default status code for not implemented errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.CriticalNotImplemented;

    private static NotImplementedError _Self = new(NOT_IMPLEMENTED);

    /// <summary>Cached instance for common not implemented errors.</summary>
    public static ref NotImplementedError Value => ref _Self;
}
