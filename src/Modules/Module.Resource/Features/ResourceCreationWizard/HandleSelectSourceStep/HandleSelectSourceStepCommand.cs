using Mediator;
using Module.Resource.Ui.ResourceCreationWizard;

namespace Module.Resource.Features.ResourceCreationWizard.HandleSelectSourceStep;

public class HandleSelectSourceStepCommand : IRequest
{
    public Guid SourceId { get; set; }
    public required MetadataForm Metadata { get; set; }
}
