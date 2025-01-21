using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public static Task<IResult> GetDockerDestinationDialog(HttpContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IResult>(new RazorHxResult<CreateDockerDestinationDialog>());
    }

    public static async Task<IResult> PostDockerDestinationDialog(
        [FromForm] PostDockerDestinationRequest request,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        await mediator.Send(
            new CreateDockerDestinationCommand { Name = request.Name, Uri = new Uri(request.Endpoint) },
            cancellationToken);

        context.Response.Headers["HX-Trigger"] = "destination-created";

        return new RazorHxResult<CreateDockerDestinationDialog>();
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
