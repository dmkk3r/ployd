using Mediator;
using Module.Destination.Contract;
using Module.Resource.Contract;
using Module.Resource.Ui.ResourceCreationWizard;
using Module.Resource.Ui.ResourceCreationWizard.DestinationMetadata;
using Module.Resource.Ui.ResourceCreationWizard.ResourceMetadata;
using Module.Source.Contract;
using Modules.Shared.Interfaces;

namespace Module.Resource.Features.Resource.PrepareCreationPlan;

public class
    PrepareCreationPlanCommandHandler : IRequestHandler<PrepareCreationPlanCommand, PrepareCreationPlanResponse>
{
    private readonly IPloydWebStore _ploydWebStore;

    public PrepareCreationPlanCommandHandler(IPloydWebStore ploydWebStore)
    {
        _ploydWebStore = ploydWebStore;
    }

    public async ValueTask<PrepareCreationPlanResponse> Handle(PrepareCreationPlanCommand request,
        CancellationToken cancellationToken)
    {
        var sourceStepForm = await _ploydWebStore.RetrieveAsync<SelectSourceStepForm>(nameof(SelectSourceStepForm));
        var createResourceStepForm =
            await _ploydWebStore.RetrieveAsync<CreateResourceStepForm>(nameof(CreateResourceStepForm));
        var destinationStepForm =
            await _ploydWebStore.RetrieveAsync<SelectDestinationStepForm>(nameof(SelectDestinationStepForm));

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
            _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.Container => await _ploydWebStore
                .RetrieveAsync<OciMetadataForm>(nameof(OciMetadataForm)),
            _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.DockerCompose => null,
            _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.PodmanCompose => null,
            _ when createResourceStepForm?.ResourceTypeId == ResourceTypes.WebAssembly => null,
            _ => null
        };

        IMetadataForm? destinationMetadata = destinationStepForm?.DestinationTypeId switch
        {
            _ when destinationStepForm?.DestinationTypeId == DestinationTypes.DockerEngine => await _ploydWebStore
                .RetrieveAsync<DockerContainerMetadataForm>(nameof(DockerContainerMetadataForm)),
            _ when destinationStepForm?.DestinationTypeId == DestinationTypes.Podman => null,
            _ when destinationStepForm?.DestinationTypeId == DestinationTypes.Wasmtime => null,
            _ => null
        };

        return new PrepareCreationPlanResponse
        {
            SourceId = sourceStepForm?.SourceId,
            SourceMetadataForm = sourceMetadata,
            ResourceTypeId = createResourceStepForm?.ResourceTypeId,
            ResourceMetadataForm = resourceMetadata,
            DestinationTypeId = destinationStepForm?.DestinationTypeId,
            DestinationMetadataForm = destinationMetadata
        };
    }
}
