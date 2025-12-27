using static Ramsha.RamshaErrorsCodes;
using System.Text.Json.Serialization;


namespace Ramsha;

public readonly record struct NetworkError(
    string Code = NETWORK_ERROR,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        IEnumerable<NamedError>? Errors = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        RamshaErrorContext? Context = null
) : IRamshaErrorResult<NetworkError>
{
    /// <summary>The default status code for network errors.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.NetworkError;

    private static NetworkError _Self = new(NETWORK_ERROR);

    /// <summary>Cached instance for common network errors.</summary>
    public static ref NetworkError Value => ref _Self;
}
