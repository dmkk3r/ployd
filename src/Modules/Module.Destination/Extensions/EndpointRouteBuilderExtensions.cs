using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Module.Destination.Endpoints;
using Module.Destination.Endpoints.Docker;

namespace Module.Destination.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapDestinationModule(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/destinations/docker", DockerDestinationEndpoints.GetDestinations);

        builder.MapPost("/destinations/docker", DockerDestinationEndpoints.CreateDockerDestination);
        builder.MapPost("/destinations/docker/containers", DockerDestinationEndpoints.CreateDockerContainer);

        return builder;
    }
}
