namespace Ramsha;

public record RamshaValueSuccessResult<TValue>(ResultStatus Status, TValue Value)
    : RamshaSuccessResult(Status),
        IRamshaValueSuccessResult<TValue>;

