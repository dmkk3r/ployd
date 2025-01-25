using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Resource.Features.Docker.CreateDockerResource;
using Module.Resource.Features.Resource.GetResourcesQuery;
using Module.Resource.Ui;
using Module.Resource.Ui.ResourceCreationWizard;
using Modules.Shared;
using Modules.Shared.Interfaces;
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
        Dictionary<Guid, string> tempSources = new() { { Guid.Empty, "Docker" }, };
        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nameof(SelectSourceStep), IsLastStep = false, IsFirstStep = true, Sources = tempSources
        });
    }

    public static async Task<IResult> ResourceCreationWizard(IPloydWebStore ploydWebStore, HttpContext context,
        CancellationToken cancellationToken)
    {
        IFormCollection? form = null;

        if (context.Request.HasFormContentType)
        {
            form = await context.Request.ReadFormAsync(cancellationToken);

            switch (form["currentStep"].ToString())
            {
                case nameof(SelectSourceStep):
                    var selectSourceStepForm = new SelectSourceStepForm { SourceId = Guid.Parse(form["source"]) };
                    await ploydWebStore.StoreAsync(nameof(SelectSourceStepForm), selectSourceStepForm);
                    break;
                case nameof(CreateResourceStep):
                    var createResourceStepForm =
                        new CreateResourceStepForm
                        {
                            Name = form["name"], ResourceType = Guid.Parse(form["resourceType"])
                        };
                    await ploydWebStore.StoreAsync(nameof(CreateResourceStepForm), createResourceStepForm);
                    break;
            }
        }

        string step = form?["currentStep"].ToString() ?? string.Empty;
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

        Dictionary<Guid, string> tempSources = new() { { Guid.Empty, "Docker" }, };

        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nextOrPrevStep, IsLastStep = isLastStep, IsFirstStep = isFirstStep, Sources = tempSources
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
