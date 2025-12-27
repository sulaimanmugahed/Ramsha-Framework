namespace Ramsha;

/// <summary>
/// Represents the status of an asynchronous job or operation.
/// </summary>
public enum JobStatus
{
    Unknown,
    Pending,
    Running,
    Paused,
    Completed,
    Failed,
    Cancelled,
    Skipped,
    Expired
}
