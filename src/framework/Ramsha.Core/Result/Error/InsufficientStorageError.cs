using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct InsufficientStorageError(
    string Code = INSUFFICIENT_STORAGE,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<InsufficientStorageError>
{
    /// <summary>The default status code for insufficient storage errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.CriticalInsufficientStorage;

    private static InsufficientStorageError _Self = new(INSUFFICIENT_STORAGE);

    /// <summary>Cached instance for common insufficient storage errors.</summary>
    public static ref InsufficientStorageError Value => ref _Self;
}
