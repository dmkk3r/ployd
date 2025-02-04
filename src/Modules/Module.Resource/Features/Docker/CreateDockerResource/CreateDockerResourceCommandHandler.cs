using Marten;
using Mediator;

namespace Module.Resource.Features.Docker.CreateDockerResource;

public class CreateDockerResourceCommandHandler(IResourceStore resourceStore)
    : IRequestHandler<CreateDockerResourceCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDockerResourceCommand request, CancellationToken cancellationToken)
    {
        await using IDocumentSession? session = resourceStore.LightweightSession();

        ContainerResource? resource = new()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Image = request.Image,
            Tag = request.Tag,
            Environment = request.Environment
        };

        session.Store(resource);
        await session.SaveChangesAsync(cancellationToken);

        return resource.Id;
    }
}
