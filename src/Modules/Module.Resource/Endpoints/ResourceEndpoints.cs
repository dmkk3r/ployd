using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Resource.Features.Docker.CreateDockerResource;
using Module.Resource.Features.Resource.GetResourcesQuery;
using Module.Resource.Ui;
using Module.Resource.Ui.ResourceCreationWizard;
using RazorHx.Results;

namespace Module.Resource.Endpoints;

public class ResourceEndpoints
{
    public static async Task<IResult> ResourcesPage(HttpContext context, CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        IReadOnlyList<Resource> resources =
            await mediator.Send(new GetResourcesQuery(), cancellationToken);

        return new RazorHxResult<ResourcesPage>(new { Resources = resources });
    }

    public static async Task<IResult> ResourcesCreationPage(HttpContext context, CancellationToken cancellationToken)
    {
        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nameof(SelectSourceStep), IsLastStep = false, IsFirstStep = true,
        });
    }

    public static async Task<IResult> ResourceCreationWizard(HttpContext context, CancellationToken cancellationToken)
    {
        string step = context.Request.Query["step"].ToString();
        string opt = context.Request.Query["opt"].ToString();

        var steps = new Dictionary<int, string>
        {
            { 1, nameof(SelectSourceStep) },
            { 2, nameof(CreateResourceStep) },
            { 3, nameof(SelectDestinationStep) },
        };

        int currentStep = steps.FirstOrDefault(x => x.Value == step).Key;

        string? nextOrPrevStep = opt switch
        {
            "next" => steps[currentStep + 1],
            "back" => steps[currentStep - 1],
            _ => null
        };

        bool isLastStep = steps.Last().Value == nextOrPrevStep;
        bool isFirstStep = steps.First().Value == nextOrPrevStep;

        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nextOrPrevStep, IsLastStep = isLastStep, IsFirstStep = isFirstStep
        });
    }

    public static async Task<IResult> CreateDockerResource(
        CreateDockerResourceRequest request,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        Guid resourceId =
            await mediator.Send(
                new CreateDockerResourceCommand
                {
                    Name = request.Name, Image = request.Image, Tag = request.Tag, Environment = request.Environment
                }, cancellationToken);

        return Results.Created("/resources", resourceId);
    }
}
