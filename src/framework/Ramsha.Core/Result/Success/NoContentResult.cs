


namespace Ramsha;


/// <summary>
/// Represents a 204 No Content success result.
/// </summary>
public readonly record struct NoContentResult : IRamshaSuccessResult<NoContentResult>
{
    /// <summary>The default status code for no content results.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.NoContent;

    private static NoContentResult _Self = default;

    /// <summary>Cached instance for common no content results.</summary>
    public static ref NoContentResult Value => ref _Self;
}
