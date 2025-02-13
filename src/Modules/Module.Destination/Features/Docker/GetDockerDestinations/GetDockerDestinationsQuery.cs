using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.Docker.GetDockerDestinations;

public class GetDockerDestinationsQuery : IRequest<IReadOnlyList<DockerDestination>>
{
}
