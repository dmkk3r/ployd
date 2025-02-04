using Mediator;
using Module.Resource.Helper;
using Module.Resource.Ui.ResourceCreationWizard;
using Modules.Shared.Interfaces;

namespace Module.Resource.Features.ResourceCreationWizard.HandleCreateResourceStep;

public class HandleCreateResourceStepCommandHandler : IRequestHandler<HandleCreateResourceStepCommand>
{
    private readonly IPloydWebStore _ploydWebStore;

    public HandleCreateResourceStepCommandHandler(IPloydWebStore ploydWebStore)
    {
        _ploydWebStore = ploydWebStore;
    }

    public async ValueTask<Unit> Handle(HandleCreateResourceStepCommand request, CancellationToken cancellationToken)
    {
        await _ploydWebStore.StoreAsync(request.Metadata.Type, request.Metadata);

        var createResourceStepForm =
            new CreateResourceStepForm { ResourceTypeId = request.ResourceTypeId };
        await _ploydWebStore.StoreAsync(nameof(CreateResourceStepForm), createResourceStepForm);

        var tempSelectDestinationForm =
            await _ploydWebStore.RetrieveAsync<SelectDestinationStepForm>(nameof(SelectDestinationStepForm));

        if (tempSelectDestinationForm == null)
        {
            await _ploydWebStore.StoreAsync(nameof(SelectDestinationStepForm),
                new SelectDestinationStepForm
                {
                    DestinationTypeId = DestinationTypesHelper
                        .ValidDestinationTypes(request.ResourceTypeId)
                        .FirstOrDefault()?.Id
                });
        }
        else
        {
            var validDestinationTypes = DestinationTypesHelper
                .ValidDestinationTypes(request.ResourceTypeId);

            if (validDestinationTypes.All(x => x.Id != tempSelectDestinationForm.DestinationTypeId))
            {
                await _ploydWebStore.StoreAsync(nameof(SelectDestinationStepForm),
                    new SelectDestinationStepForm { DestinationTypeId = validDestinationTypes.FirstOrDefault()?.Id });
            }
        }

        return Unit.Value;
    }
}
