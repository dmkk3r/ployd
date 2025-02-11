using Mediator;
using Modules.Shared.Interfaces;

namespace Module.Resource.Features.ResourceCreationWizard.HandleSelectDestinationStep;

public class HandleSelectDestinationStepCommandHandler : IRequestHandler<HandleSelectDestinationStepCommand>
{
    private readonly IPloydWebStore _ploydWebStore;

    public HandleSelectDestinationStepCommandHandler(IPloydWebStore ploydWebStore)
    {
        _ploydWebStore = ploydWebStore;
    }

    public async ValueTask<Unit> Handle(HandleSelectDestinationStepCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
