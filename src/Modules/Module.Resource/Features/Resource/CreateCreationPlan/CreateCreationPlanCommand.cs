using Mediator;
using Module.Resource.Ui.ResourceCreationWizard;

namespace Module.Resource.Features.Resource.CreateCreationPlan;

public class CreateCreationPlanCommand : IRequest
{
    public Guid? SourceId { get; set; }
    public required MetadataForm? SourceMetadataForm { get; set; }
    public Guid? ResourceTypeId { get; set; }
    public required MetadataForm? ResourceMetadataForm { get; set; }
    public Guid? DestinationTypeId { get; set; }
    public required MetadataForm? DestinationMetadataForm { get; set; }
}
