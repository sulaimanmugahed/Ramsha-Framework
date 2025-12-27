namespace Ramsha;

[IRamshaResultJsonConverter]
public interface IRamshaResult
{
    public ResultStatus Status { get; }
}

public interface IRamshaResult<T> : IRamshaResult
    where T : IRamshaResult<T>
{
    ResultStatus IRamshaResult.Status => T.DefaultStatus;
    public abstract static ResultStatus DefaultStatus { get; }
}

