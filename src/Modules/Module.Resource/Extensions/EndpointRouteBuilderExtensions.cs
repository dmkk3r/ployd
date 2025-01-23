using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Module.Resource.Endpoints;

namespace Module.Resource.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapResourceModule(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/resources", ResourceEndpoints.GetResources);
        builder.MapPost("/resources/docker", ResourceEndpoints.CreateDockerResource);

        return builder;
    }
}
