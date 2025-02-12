using Mediator;
using Module.Resource.Ui.ResourceCreationWizard;
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
        await _ploydWebStore.StoreAsync(request.Metadata.Type, request.Metadata);

        var selectDestinationStepForm = new SelectDestinationStepForm() { DestinationTypeId = request.DestinationId };
        await _ploydWebStore.StoreAsync(nameof(SelectDestinationStepForm), selectDestinationStepForm);

        return Unit.Value;
    }
}
