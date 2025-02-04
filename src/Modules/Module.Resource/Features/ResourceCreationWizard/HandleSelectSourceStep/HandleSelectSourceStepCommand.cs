using Mediator;

namespace Module.Resource.Features.ResourceCreationWizard.HandleSelectSourceStep;

public class HandleSelectSourceStepCommand : IRequest
{
    public Guid SourceId { get; set; }
}
