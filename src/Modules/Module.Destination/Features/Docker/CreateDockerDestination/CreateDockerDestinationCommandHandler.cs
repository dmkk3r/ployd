using Marten;
using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.Docker.CreateDockerDestination;

public class CreateDockerDestinationCommandHandler(IDestinationStore store)
    : IRequestHandler<CreateDockerDestinationCommand>
{
    public async ValueTask<Unit> Handle(CreateDockerDestinationCommand request, CancellationToken cancellationToken)
    {
        await using IDocumentSession? session = store.LightweightSession();

        DockerDestination? destination = new() { Id = Guid.CreateVersion7(), Name = request.Name, Uri = request.Uri };

        session.Store(destination);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
