using System.Text.Json;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.BackgroundProcessing.Contract;
using Module.Destination.Contract;
using Module.Resource.Contract;
using Module.Resource.Features.Docker.CreateDockerResource;
using Module.Resource.Features.Resource.GetResourcesQuery;
using Module.Resource.Features.Resource.PrepareCreationPlan;
using Module.Resource.Features.ResourceCreationWizard.HandleCreateResourceStep;
using Module.Resource.Features.ResourceCreationWizard.HandleSelectSourceStep;
using Module.Resource.Ui;
using Module.Resource.Ui.ResourceCreationWizard;
using Module.Resource.Ui.ResourceCreationWizard.DestinationMetadata;
using Module.Resource.Ui.ResourceCreationWizard.ResourceMetadata;
using Module.Resource.Ui.ResourceCreationWizard.SourceMetadata;
using Module.Source.Contract;
using Modules.Shared.Interfaces;
using RazorHx.Results;

namespace Module.Resource.Endpoints;

public class ResourceEndpoints
{
    public static async Task<IResult> ResourcesPage(IPloydWebStore ploydWebStore, HttpContext context,
        CancellationToken cancellationToken)
    {
        IMediator? mediator = context.RequestServices.GetRequiredService<IMediator>();

        IReadOnlyList<Resource> resources =
            await mediator.Send(new GetResourcesQuery(), cancellationToken);

        await ploydWebStore.ClearAsync(nameof(SelectSourceStepForm));
        await ploydWebStore.ClearAsync(nameof(CreateResourceStepForm));
        await ploydWebStore.ClearAsync(nameof(ContainerMetadataForm));
        await ploydWebStore.ClearAsync(nameof(DockerEngineMetadataForm));
        await ploydWebStore.ClearAsync(nameof(SelectDestinationStepForm));

        return new RazorHxResult<ResourcesPage>(new { Resources = resources });
    }

    public static async Task<IResult> ResourcesCreationPage(IPloydWebStore ploydWebStore, HttpContext context,
        CancellationToken cancellationToken)
    {
        await ploydWebStore.ClearAsync(nameof(SelectSourceStepForm));
        await ploydWebStore.ClearAsync(nameof(CreateResourceStepForm));
        await ploydWebStore.ClearAsync(nameof(ContainerMetadataForm));
        await ploydWebStore.ClearAsync(nameof(DockerEngineMetadataForm));
        await ploydWebStore.ClearAsync(nameof(SelectDestinationStepForm));

        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nameof(SelectSourceStep), IsLastStep = false, IsFirstStep = true
        });
    }

    public static async Task<IResult> ResourceCreationWizard(IPloydWebStore ploydWebStore, IMediator mediator,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        IFormCollection? form = null;

        if (context.Request.HasFormContentType)
        {
            form = await context.Request.ReadFormAsync(cancellationToken);

            switch (form["currentStep"].ToString())
            {
                case nameof(SelectSourceStep):
                    Guid sourceId = string.IsNullOrEmpty(form["sourceId"].ToString())
                        ? throw new InvalidOperationException("SourceId is required.")
                        : Guid.Parse(form["sourceId"].ToString());

                    MetadataForm? sourceMetadataForm;

                    switch (sourceId)
                    {
                        case var _ when sourceId == SourceTypes.Git:
                        case var _ when sourceId == SourceTypes.GitHub:
                        case var _ when sourceId == SourceTypes.GitLab:
                        case var _ when sourceId == SourceTypes.DockerHub:
                            sourceMetadataForm = new DockerhubMetadataForm
                            {
                                ImageName = form["image-name"].ToString(), ImageTag = form["image-tag"].ToString()
                            };
                            break;
                        case var _ when sourceId == SourceTypes.Ghcr:
                        default:
                            sourceMetadataForm = null;
                            break;
                    }

                    await mediator.Send(new HandleSelectSourceStepCommand { SourceId = sourceId, Metadata = sourceMetadataForm}, cancellationToken);
                    break;
                case nameof(CreateResourceStep):
                    Guid resourceTypeId = string.IsNullOrEmpty(form["resourceTypeId"].ToString())
                        ? throw new InvalidOperationException("ResourceTypeId is required.")
                        : Guid.Parse(form["resourceTypeId"].ToString());

                    MetadataForm? resourceMetadataForm;

                    switch (resourceTypeId)
                    {
                        case var _ when resourceTypeId == ResourceTypes.Container:
                            resourceMetadataForm = new ContainerMetadataForm { Name = form["container-name"].ToString() };
                            break;
                        case var _ when resourceTypeId == ResourceTypes.DockerCompose:
                        case var _ when resourceTypeId == ResourceTypes.PodmanCompose:
                        case var _ when resourceTypeId == ResourceTypes.WebAssembly:
                        default:
                            resourceMetadataForm = null;
                            break;
                    }

                    await mediator.Send(
                        new HandleCreateResourceStepCommand { ResourceTypeId = resourceTypeId, Metadata = resourceMetadataForm },
                        cancellationToken);

                    break;
                case nameof(SelectDestinationStep):
                    Guid? destinationTypeId = string.IsNullOrEmpty(form["destinationTypeId"].ToString())
                        ? null
                        : Guid.Parse(form["destinationTypeId"].ToString());

                    switch (destinationTypeId)
                    {
                        case var _ when destinationTypeId == DestinationTypes.DockerEngine:
                            var dockerEngineMetadataForm = new DockerEngineMetadataForm
                            {
                            };
                            await ploydWebStore.StoreAsync(nameof(DockerEngineMetadataForm),
                                dockerEngineMetadataForm);
                            break;
                        case var _ when destinationTypeId == DestinationTypes.Podman:
                        case var _ when destinationTypeId == DestinationTypes.Wasmtime:
                        default:
                            break;
                    }

                    var selectDestinationStepForm =
                        new SelectDestinationStepForm { DestinationTypeId = destinationTypeId };
                    await ploydWebStore.StoreAsync(nameof(SelectDestinationStepForm), selectDestinationStepForm);
                    break;
            }
        }

        string step = form?["currentStep"].ToString() ?? string.Empty;
        string opt = context.Request.Query["opt"].ToString();

        var steps = new Dictionary<int, string>
        {
            { 1, nameof(SelectSourceStep) }, { 2, nameof(CreateResourceStep) }, { 3, nameof(SelectDestinationStep) }
        };

        int currentStep = steps.FirstOrDefault(x => x.Value == step).Key;
        bool isFinish = opt == "finish";

        if (isFinish)
        {
            var prepareCreationPlanResponse =
                await mediator.Send(new PrepareCreationPlanCommand(), cancellationToken);

            await mediator.Send(
                new QueueBackgroundJobCommand
                {
                    Group = Guid.NewGuid().ToString(),
                    PayloadType = typeof(CreateDockerResourceCommand),
                    Payload = JsonSerializer.Serialize(new CreateDockerResourceCommand
                    {
                        Name = "ployd", Image = "ployd", Tag = "latest"
                    })
                }, cancellationToken);

            context.Response.Headers["HX-Trigger"] = "resource-creation-wizard-finished";

            return Results.Ok();
        }

        string? nextOrPrevStep = opt switch
        {
            "next" => steps[currentStep + 1],
            "back" => steps[currentStep - 1],
            _ => null
        };

        return new RazorHxResult<ResourcesCreationPage>(new
        {
            CurrentStep = nextOrPrevStep,
            IsLastStep = steps.Last().Value == nextOrPrevStep,
            IsFirstStep = steps.First().Value == nextOrPrevStep
        });
    }

    public static Task<IResult> ResourceCreationWizardMetadata(HttpContext context,
        CancellationToken cancellationToken)
    {
        string id = context.Request.Query["sourceId"].ToString();

        if (string.IsNullOrEmpty(id))
        {
            id = context.Request.Query["resourceTypeId"].ToString();
        }

        if (string.IsNullOrEmpty(id))
        {
            id = context.Request.Query["destinationTypeId"].ToString();
        }

        if (string.IsNullOrEmpty(id))
        {
            return Task.FromResult(Results.NoContent());
        }

        if (!Guid.TryParse(id, out Guid guid))
        {
            return Task.FromResult(Results.BadRequest());
        }

        return Task.FromResult(id switch
        {
            _ when guid == SourceTypes.Git => Results.NoContent(),
            _ when guid == SourceTypes.GitHub => Results.NoContent(),
            _ when guid == SourceTypes.GitLab => Results.NoContent(),
            _ when guid == SourceTypes.DockerHub => new RazorHxResult<DockerhubMetadata>(),
            _ when guid == SourceTypes.Ghcr => Results.NoContent(),
            _ when guid == ResourceTypes.DockerCompose => Results.NoContent(),
            _ when guid == ResourceTypes.PodmanCompose => Results.NoContent(),
            _ when guid == ResourceTypes.Container => new RazorHxResult<ContainerMetadata>(),
            _ when guid == ResourceTypes.WebAssembly => Results.NoContent(),
            _ when guid == DestinationTypes.DockerEngine => new RazorHxResult<DockerEngineMetadata>(),
            _ when guid == DestinationTypes.Podman => new RazorHxResult<PodmanEngineMetadata>(),
            _ when guid == DestinationTypes.Wasmtime => Results.NoContent(),
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
