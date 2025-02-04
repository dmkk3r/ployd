using Mediator;
using Module.Resource.Helper;
using Module.Resource.Ui.ResourceCreationWizard;
using Modules.Shared.Interfaces;

namespace Module.Resource.Features.ResourceCreationWizard.HandleSelectSourceStep;

public class HandleSelectSourceStepCommandHandler : IRequestHandler<HandleSelectSourceStepCommand>
{
    private readonly IPloydWebStore _ploydWebStore;

    public HandleSelectSourceStepCommandHandler(IPloydWebStore ploydWebStore)
    {
        _ploydWebStore = ploydWebStore;
    }

    public async ValueTask<Unit> Handle(HandleSelectSourceStepCommand request, CancellationToken cancellationToken)
    {
        var selectSourceStepForm = new SelectSourceStepForm { SourceId = request.SourceId };
        await _ploydWebStore.StoreAsync(nameof(SelectSourceStepForm), selectSourceStepForm);

        var tempCreateResourceStepForm =
            await _ploydWebStore.RetrieveAsync<CreateResourceStepForm>(nameof(CreateResourceStepForm));

        if (tempCreateResourceStepForm == null)
        {
            await _ploydWebStore.StoreAsync(nameof(CreateResourceStepForm),
                new CreateResourceStepForm
                {
                    ResourceTypeId = ResourceTypesHelper
                        .ValidResourceTypes(request.SourceId)
                        .FirstOrDefault()?.Id
                });
        }
        else
        {
            var validResourceTypes = ResourceTypesHelper
                .ValidResourceTypes(request.SourceId);

            if (validResourceTypes.All(x => x.Id != tempCreateResourceStepForm.ResourceTypeId))
            {
                await _ploydWebStore.StoreAsync(nameof(CreateResourceStepForm),
                    new CreateResourceStepForm { ResourceTypeId = validResourceTypes.FirstOrDefault()?.Id });
            }
        }

        return Unit.Value;
    }
}
