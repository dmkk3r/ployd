using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Module.Destination.Contract;
using Module.Resource.Contract;
using Module.Resource.Features.Docker.CreateDockerResource;
using Module.Resource.Features.Resource.CreateCreationPlan;
using Module.Resource.Features.Resource.GetResourcesQuery;
using Module.Resource.Ui;
using Module.Resource.Ui.ResourceCreationWizard;
using Module.Resource.Ui.ResourceCreationWizard.DestinationMetadata;
using Module.Resource.Ui.ResourceCreationWizard.ResourceMetadata;
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
        await ploydWebStore.ClearAsync(nameof(OciMetadataForm));
        await ploydWebStore.ClearAsync(nameof(DockerContainerMetadataForm));
        await ploydWebStore.ClearAsync(nameof(SelectDestinationStepForm));

        return new RazorHxResult<ResourcesPage>(new { Resources = resources });
    }

    public static async Task<IResult> ResourcesCreationPage(IPloydWebStore ploydWebStore, HttpContext context,
        CancellationToken cancellationToken)
    {
        await ploydWebStore.ClearAsync(nameof(SelectSourceStepForm));
        await ploydWebStore.ClearAsync(nameof(CreateResourceStepForm));
        await ploydWebStore.ClearAsync(nameof(OciMetadataForm));
        await ploydWebStore.ClearAsync(nameof(DockerContainerMetadataForm));
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

                    switch (destinationTypeId)
                    {
                        case var _ when destinationTypeId == DestinationTypes.DockerContainer:
                            var dockerContainerMetadataForm = new DockerContainerMetadataForm
                            {
                                Name = form["container-name"].ToString()
                            };
                            await ploydWebStore.StoreAsync(nameof(DockerContainerMetadataForm),
                                dockerContainerMetadataForm);
                            break;
                        case var _ when destinationTypeId == DestinationTypes.PodmanContainer:
                        case var _ when destinationTypeId == DestinationTypes.WebAssembly:
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
            var sourceStepForm = await ploydWebStore.RetrieveAsync<SelectSourceStepForm>(nameof(SelectSourceStepForm));
            var createResourceStepForm =
                await ploydWebStore.RetrieveAsync<CreateResourceStepForm>(nameof(CreateResourceStepForm));
            var destinationStepForm =
                await ploydWebStore.RetrieveAsync<SelectDestinationStepForm>(nameof(SelectDestinationStepForm));

            IMetadataForm? sourceMetadata = sourceStepForm?.SourceId switch
            {
                _ when sourceStepForm?.SourceId == SourceTypes.Git => null,
                _ when sourceStepForm?.SourceId == SourceTypes.GitHub => null,
                _ when sourceStepForm?.SourceId == SourceTypes.GitLab => null,
                _ when sourceStepForm?.SourceId == SourceTypes.DockerHub => null,
                _ when sourceStepForm?.SourceId == SourceTypes.Ghcr => null,
                _ => null
            };

            IMetadataForm? resourceMetadata = createResourceStepForm?.ResourceTypeId switch
            {
                _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.Dockerfile => null,
                _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.DockerCompose => null,
                _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.PodmanCompose => null,
                _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.OciImage => await ploydWebStore
                    .RetrieveAsync<OciMetadataForm>(nameof(OciMetadataForm)),
                _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.WebAssembly => null,
                _ => null
            };

            IMetadataForm? destinationMetadata = destinationStepForm?.DestinationTypeId switch
            {
                _ when destinationStepForm?.DestinationTypeId == DestinationTypes.DockerContainer => await ploydWebStore
                    .RetrieveAsync<DockerContainerMetadataForm>(nameof(DockerContainerMetadataForm)),
                _ when destinationStepForm?.DestinationTypeId == DestinationTypes.PodmanContainer => null,
                _ when destinationStepForm?.DestinationTypeId == DestinationTypes.WebAssembly => null,
                _ => null
            };

            await mediator.Send(
                new CreateCreationPlanCommand
                {
                    SourceId = sourceStepForm?.SourceId ?? Guid.Empty,
                    SourceMetadataForm = sourceMetadata,
                    ResourceTypeId = createResourceStepForm?.ResourceTypeId ?? Guid.Empty,
                    ResourceMetadataForm = resourceMetadata,
                    DestinationTypeId = destinationStepForm?.DestinationTypeId ?? Guid.Empty,
                    DestinationMetadataForm = destinationMetadata
                },
                cancellationToken);

            context.Response.Headers["HX-Trigger"] = "resource-creation-wizard-finished";

            return Results.Ok();
        }

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
            _ when guid == SourceTypes.DockerHub => Results.NoContent(),
            _ when guid == SourceTypes.Ghcr => Results.NoContent(),
            _ when guid == ResourceTypes.Dockerfile => Results.NoContent(),
            _ when guid == ResourceTypes.DockerCompose => Results.NoContent(),
            _ when guid == ResourceTypes.PodmanCompose => Results.NoContent(),
            _ when guid == ResourceTypes.OciImage => new RazorHxResult<OciMetadata>(),
            _ when guid == ResourceTypes.WebAssembly => Results.NoContent(),
            _ when guid == DestinationTypes.DockerContainer => new RazorHxResult<DockerContainerMetadata>(),
            _ when guid == DestinationTypes.PodmanContainer => Results.NoContent(),
            _ when guid == DestinationTypes.WebAssembly => Results.NoContent(),
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
