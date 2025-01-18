using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Module.Destination.Endpoints;

namespace Module.Destination.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapDestinationModule(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/destinations/docker", DestinationEndpoints.CreateDockerDestination);
        builder.MapPost("/destinations/docker/containers", DestinationEndpoints.CreateDockerContainer);

        return builder;
    }
}
