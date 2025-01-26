using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Resource.Contract;
using Module.Resource.Features.Docker.CreateDockerResource;
using Module.Resource.Features.Resource.GetResourcesQuery;
using Module.Resource.Ui;
using Module.Resource.Ui.ResourceCreationWizard;
using Module.Resource.Ui.ResourceCreationWizard.ResourceTypes;
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

    public static Task<IResult> ResourcesCreationPage(HttpContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<IResult>(new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nameof(SelectSourceStep), IsLastStep = false, IsFirstStep = true
        }));
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
                    Guid? sourceId = string.IsNullOrEmpty(form["sourceId"].ToString())
                        ? null
                        : Guid.Parse(form["sourceId"].ToString());

                    var selectSourceStepForm = new SelectSourceStepForm { SourceId = sourceId };
                    await ploydWebStore.StoreAsync(nameof(SelectSourceStepForm), selectSourceStepForm);
                    break;
                case nameof(CreateResourceStep):
                    Guid? resourceTypeId = string.IsNullOrEmpty(form["resourceTypeId"].ToString())
                        ? null
                        : Guid.Parse(form["resourceTypeId"].ToString());

                    switch (resourceTypeId)
                    {
                        case var _ when resourceTypeId == ResourceTypes.Dockerfile:
                        case var _ when resourceTypeId == ResourceTypes.DockerCompose:
                        case var _ when resourceTypeId == ResourceTypes.PodmanCompose:
                            break;
                        case var _ when resourceTypeId == ResourceTypes.OciImage:
                            var ociMetadataForm = new OciMetadataForm
                            {
                                Image = form["image"].ToString(), Tag = form["tag"].ToString()
                            };
                            await ploydWebStore.StoreAsync(nameof(OciMetadataForm), ociMetadataForm);
                            break;
                        case var _ when resourceTypeId == ResourceTypes.WebAssembly:
                        default:
                            break;
                    }

                    var createResourceStepForm =
                        new CreateResourceStepForm { ResourceTypeId = resourceTypeId };
                    await ploydWebStore.StoreAsync(nameof(CreateResourceStepForm), createResourceStepForm);
                    break;
                case nameof(SelectDestinationStep):
                    Guid? destinationTypeId = string.IsNullOrEmpty(form["destinationTypeId"].ToString())
                        ? null
                        : Guid.Parse(form["destinationTypeId"].ToString());

                    var selectDestinationStepForm =
                        new SelectDestinationStepForm() { DestinationTypeId = destinationTypeId };
                    await ploydWebStore.StoreAsync(nameof(SelectDestinationStepForm), selectDestinationStepForm);
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

        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nextOrPrevStep, IsLastStep = isLastStep, IsFirstStep = isFirstStep
        });
    }

    public static Task<IResult> ResourceCreationWizardMetadata(HttpContext context,
        CancellationToken cancellationToken)
    {
        string resourceTypeId = context.Request.Query["resourceTypeId"].ToString();

        if (!Guid.TryParse(resourceTypeId, out Guid id))
        {
            return Task.FromResult(Results.NoContent());
        }

        return Task.FromResult(id switch
        {
            _ when id == ResourceTypes.Dockerfile => Results.NoContent(),
            _ when id == ResourceTypes.DockerCompose => Results.NoContent(),
            _ when id == ResourceTypes.PodmanCompose => Results.NoContent(),
            _ when id == ResourceTypes.OciImage => new RazorHxResult<OciMetadata>(),
            _ when id == ResourceTypes.WebAssembly => Results.NoContent(),
            _ => Results.NoContent()
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
