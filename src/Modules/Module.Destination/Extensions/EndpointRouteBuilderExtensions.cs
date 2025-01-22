using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Module.Destination.Endpoints.Docker;

namespace Module.Destination.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapDestinationModule(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/destinations/docker", DockerDestinationEndpoints.GetDestinations);
        builder.MapGet("/destinations/docker/create", DockerDestinationEndpoints.GetDockerDestinationDialog);
        builder.MapPost("/destinations/docker/create", DockerDestinationEndpoints.PostDockerDestinationDialog)
            .DisableAntiforgery();
        builder.MapPost("/destinations/docker/test", DockerDestinationEndpoints.TestDockerDestination);
        builder.MapPost("/destinations/docker/containers", DockerDestinationEndpoints.CreateDockerContainer);

        return builder;
    }
}
