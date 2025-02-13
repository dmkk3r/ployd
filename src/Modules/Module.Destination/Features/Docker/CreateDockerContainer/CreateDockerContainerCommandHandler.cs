using Docker.DotNet;
using Docker.DotNet.Models;
using Marten;
using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.Docker.CreateDockerContainer;

public class CreateDockerContainerCommandHandler(IDestinationStore store)
    : IRequestHandler<CreateDockerContainerCommand>
{
    public async ValueTask<Unit> Handle(CreateDockerContainerCommand request, CancellationToken cancellationToken)
    {
        await using IDocumentSession session = store.LightweightSession();

        DockerDestination? destination =
            await session.LoadAsync<DockerDestination>(request.DestinationId, cancellationToken);

        if (destination is null)
        {
            throw new InvalidOperationException("Destination not found.");
        }

        DockerClient client = new DockerClientConfiguration(destination.Uri).CreateClient();

        IList<ImagesListResponse>? images =
            await client.Images.ListImagesAsync(new ImagesListParameters(), cancellationToken);

        if (!images.Any(x => x.RepoTags.Contains(request.Image + ":" + request.Tag)))
        {
            await client.Images.CreateImageAsync(
                new ImagesCreateParameters { FromImage = request.Image, Tag = request.Tag },
                new AuthConfig(),
                new Progress<JSONMessage>(), cancellationToken);
        }

        await client.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Name = request.Name,
                Image = request.Image + ":" + request.Tag,
                Env = request.Environment?.Select(x => $"{x.Key}={x.Value}").ToList(),
                HostConfig = new HostConfig { AutoRemove = true }
            }, cancellationToken);

        return Unit.Value;
    }
}
