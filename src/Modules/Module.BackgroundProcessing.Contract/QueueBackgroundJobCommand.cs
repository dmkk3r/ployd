using Mediator;

namespace Module.BackgroundProcessing.Contract;

public class QueueBackgroundJobCommand : ICommand
{
    public string? Group { get; init; }
    public required Type PayloadType { get; init; }
    public required string Payload { get; init; }
}
