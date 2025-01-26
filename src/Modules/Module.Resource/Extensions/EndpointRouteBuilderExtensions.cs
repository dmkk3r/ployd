using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Module.Resource.Endpoints;

namespace Module.Resource.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapResourceModule(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/resources", ResourceEndpoints.ResourcesPage);
        builder.MapGet("/resources/create", ResourceEndpoints.ResourcesCreationPage);
        builder.MapMethods("/resources/create/wizard", ["GET", "POST"], ResourceEndpoints.ResourceCreationWizard);
        builder.MapGet("/resources/create/wizard/metadata", ResourceEndpoints.ResourceCreationWizardMetadata);
        builder.MapPost("/resources/docker", ResourceEndpoints.CreateDockerResource);

        return builder;
    }
}
