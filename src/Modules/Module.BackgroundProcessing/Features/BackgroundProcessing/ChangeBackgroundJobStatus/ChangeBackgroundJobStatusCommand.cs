using Mediator;

namespace Module.BackgroundProcessing.Features.BackgroundProcessing.ChangeBackgroundJobStatus;

public class ChangeBackgroundJobStatusCommand : ICommand
{
    public required Guid Id { get; set; }
    public required BackgroundJobStatus Status { get; set; }
}
