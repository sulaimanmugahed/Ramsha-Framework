namespace Ramsha;

public record AcceptedResult(JobInfo Value)
 : SuccessResult<JobInfo>(Value, RamshaResultStatus.Accepted)
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
    {

    }
}
