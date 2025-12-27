namespace Ramsha;

/// <summary>
/// Represents a response indicating a request was accepted for processing (e.g., async jobs).
/// </summary>
/// <param name="Job">The accepted job information.</param>
public readonly record struct AcceptedResult(JobInfo Value) : IRamshaValueSuccessResult<AcceptedResult, JobInfo>
{
    public AcceptedResult(
        string jobId,
        string? description = null,
        JobStatus status = JobStatus.Unknown,
        DateTime? estimatedToStartAt = null,
        TimeSpan? estimatedRunningDuration = null
    ) : this(
        new JobInfo(jobId, description, status, estimatedToStartAt, estimatedRunningDuration)
    )
    { }

    public static ResultStatus DefaultStatus => ResultStatus.Accepted;
}
