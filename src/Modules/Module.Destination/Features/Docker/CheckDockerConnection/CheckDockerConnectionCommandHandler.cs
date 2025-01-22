using Docker.DotNet;
using Mediator;

namespace Module.Destination.Features.Docker.CheckDockerConnection;

public class CheckDockerConnectionCommandHandler : IRequestHandler<CheckDockerConnectionCommand, string?>
{
    public async ValueTask<string?> Handle(CheckDockerConnectionCommand request, CancellationToken cancellationToken)
    {
        DockerClient client = new DockerClientConfiguration(new Uri(request.Endpoint)).CreateClient();

        try
        {
            var version = await client.System.GetVersionAsync(cancellationToken);
            return version.Version;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
