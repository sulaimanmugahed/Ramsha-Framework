namespace Ramsha;

/// <summary>
/// Represents a 200 OK success result that carries a value.
/// </summary>
public readonly record struct ValueSuccessResult<TValue>(TValue Value)
    : IRamshaValueSuccessResult<ValueSuccessResult<TValue>, TValue>
{
    /// <summary>The default status code for value success results.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.OK;

    public static implicit operator Task<IRamshaResult>(ValueSuccessResult<TValue> result)
     => Task.FromResult<IRamshaResult>(result);
}

public readonly record struct SuccessResult
    : IRamshaSuccessResult<SuccessResult>
{
    /// <summary>The default status code for value success results.</summary>
    public static ResultStatus DefaultStatus => ResultStatus.OK;
    private static SuccessResult _Self = new();

    /// <summary>Cached instance for common no content results.</summary>
    public static ref SuccessResult Value => ref _Self;
}
