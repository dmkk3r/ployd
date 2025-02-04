namespace Module.BackgroundProcessing;

public class BackgroundJob
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public BackgroundJobStatus Status { get; set; }
    public string? Group { get; set; }
    public required string PayloadType { get; set; }
    public required string Payload { get; set; }
}

public enum BackgroundJobStatus
{
    Queued,
    Running,
    Completed,
    Failed
}
