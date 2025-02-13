using Marten;
using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.Docker.GetDockerDestinations;

public class GetDockerDestinationsQueryHandler(IDestinationStore store)
    : IRequestHandler<GetDockerDestinationsQuery, IReadOnlyList<DockerDestination>>
{
    public async ValueTask<IReadOnlyList<DockerDestination>> Handle(GetDockerDestinationsQuery request,
        CancellationToken cancellationToken)
    {
        await using IQuerySession session = store.QuerySession();

        return await session.Query<DockerDestination>().ToListAsync(cancellationToken);
    }
}
