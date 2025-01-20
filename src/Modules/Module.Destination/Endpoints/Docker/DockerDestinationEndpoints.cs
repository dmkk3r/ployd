using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Destination.Features.Docker.CreateDockerContainer;
using Module.Destination.Features.Docker.CreateDockerDestination;
using Module.Destination.Features.Docker.GetDockerDestinations;
using Module.Destination.Ui.Docker;
using RazorHx.Results;

namespace Module.Destination.Endpoints.Docker;

public class DockerDestinationEndpoints
{
    public static async Task<IResult> GetDestinations(HttpContext context, CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        IReadOnlyList<DockerDestination> destinations =
            await mediator.Send(new GetDockerDestinationsQuery(), cancellationToken);

        return new RazorHxResult<DockerDestinationPage>(new { Destinations = destinations });
    }

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
