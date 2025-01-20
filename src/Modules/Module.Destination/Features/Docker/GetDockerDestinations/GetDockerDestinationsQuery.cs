using Mediator;

namespace Module.Destination.Features.Docker.GetDockerDestinations;

public class GetDockerDestinationsQuery : IRequest<IReadOnlyList<DockerDestination>>
{
}
