using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Destination.Features.CreateDockerContainer;
using Module.Destination.Features.CreateDockerDestination;

namespace Module.Destination.Endpoints;

public class DestinationEndpoints
{
    public static async Task<IResult> CreateDockerDestination(
        CreateDockerDestinationRequest request,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        Unit resourceId =
            await mediator.Send(
                new CreateDockerDestinationCommand { Name = request.Name, Uri = request.Uri }, cancellationToken);

        return Results.Created("/destinations", resourceId);
    }

    public static async Task<IResult> CreateDockerContainer(
        CreateDockerContainerRequest request,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        Unit resourceId =
            await mediator.Send(
                new CreateDockerContainerCommand
                {
                    DestinationId = request.DestinationId,
                    Name = request.Name,
                    Image = request.Image,
                    Tag = request.Tag,
                    Environment = request.Environment
                }, cancellationToken);

        return Results.Created("/destinations/docker/containers", resourceId);
    }
}
