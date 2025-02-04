using Mediator;
using Module.Resource.Ui.ResourceCreationWizard;

namespace Module.Resource.Features.ResourceCreationWizard.HandleCreateResourceStep;

public class HandleCreateResourceStepCommand : IRequest
{
    public Guid ResourceTypeId { get; set; }
    public required MetadataForm Metadata { get; set; }
}
