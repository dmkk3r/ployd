using Mediator;

namespace Module.Destination.Features.Docker.CheckDockerConnection;

public class CheckDockerConnectionCommand : IRequest<string?>
{
    public required string Endpoint { get; set; }
}
