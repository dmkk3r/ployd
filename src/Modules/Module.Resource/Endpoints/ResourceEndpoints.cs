using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Resource.Features.Docker.CreateDockerResource;

namespace Module.Resource.Endpoints;

public class ResourceEndpoints
{
    public static async Task<IResult> CreateDockerResource(
        CreateDockerResourceRequest request,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        var resourceId =
            await mediator.Send(
                new CreateDockerResourceCommand
                {
                    Name = request.Name, Image = request.Image, Tag = request.Tag, Environment = request.Environment
                }, cancellationToken);

        return Results.Created("/resources", resourceId);
    }
}
