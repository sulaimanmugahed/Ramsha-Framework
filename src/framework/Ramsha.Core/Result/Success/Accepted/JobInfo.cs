namespace Ramsha;

/// <summary>
/// Represents information about an accepted asynchronous job or operation.
/// </summary>
/// <param name="JobId">The unique identifier for the accepted job.</param>
/// <param name="Description">A human-readable description of the job (optional).</param>
/// <param name="EstimatedRunningDuration">The estimated completion time for the job (optional).</param>
/// <param name="Status">The current status of the job (optional).</param>
public record JobInfo(
    string JobId,
    string? Description = null,
    JobStatus Status = JobStatus.Unknown,
    DateTime? EstimatedToStartAt = null,
    TimeSpan? EstimatedRunningDuration = null
);
