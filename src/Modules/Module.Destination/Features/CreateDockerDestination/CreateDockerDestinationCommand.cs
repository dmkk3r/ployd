using Mediator;

namespace Module.Destination.Features.CreateDockerDestination;

public class CreateDockerDestinationCommand : IRequest
{
    public required string Name { get; set; }
    public required Uri Uri { get; set; }
}
