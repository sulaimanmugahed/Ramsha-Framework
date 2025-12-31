namespace Ramsha;

/// <summary>
/// Represents a 200 OK success result that carries a value.
/// </summary>
public record SuccessResult<TValue>(TValue Value, RamshaResultStatus Status = RamshaResultStatus.OK)
    : RamshaResult<TValue>(Status), IRamshaValueSuccessResult
{
    public override bool Succeeded => true;
    public override TValue? Value { get; } = Value;
    object IRamshaValueSuccessResult.Value => Value!;
    public static implicit operator Task<IRamshaResult>(SuccessResult<TValue> result)
     => Task.FromResult<IRamshaResult>(result);

    // public static implicit operator SuccessResult<TValue>(TValue result)
    // => new(result);
}

public record SuccessResult(RamshaResultStatus Status = RamshaResultStatus.OK)
    : RamshaResult(Status), IRamshaSuccessResult
{
    public override bool Succeeded => true;
}
