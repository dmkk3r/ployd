using Marten;
using Mediator;

namespace Module.Destination.Features.CreateDockerDestination;

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
