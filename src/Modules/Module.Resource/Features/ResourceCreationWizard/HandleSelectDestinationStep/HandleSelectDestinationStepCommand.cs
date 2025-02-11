using Mediator;
using Module.Resource.Ui.ResourceCreationWizard;

namespace Module.Resource.Features.ResourceCreationWizard.HandleSelectDestinationStep;

public class HandleSelectDestinationStepCommand : IRequest
{
    public Guid DestinationId { get; set; }
    public required MetadataForm Metadata { get; set; }
}
