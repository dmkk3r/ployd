using Module.Destination.Contract;
using Module.Resource.Contract;

namespace Module.Resource.Ui.ResourceCreationWizard.Helper;

public class DestinationTypesHelper
{
    public static List<DestinationTypeUi> ValidDestinationTypes(Guid? resourceTypeId, string? group = null)
    {
        List<DestinationTypeUi> validDestinationTypes;

        if (group == null)
        {
            validDestinationTypes = DestinationTypesUi.AllGrouped()
                .SelectMany(rt => rt.Value).ToList();
        }
        else
        {
            validDestinationTypes = DestinationTypesUi.AllGrouped().Where(rt => rt.Key == group)
                .SelectMany(rt => rt.Value).ToList();
        }

        switch (resourceTypeId)
        {
            case var _ when resourceTypeId == ResourceTypes.Container:
            case var _ when resourceTypeId == ResourceTypes.DockerCompose:
            case var _ when resourceTypeId == ResourceTypes.PodmanCompose:
                return validDestinationTypes.Where(dt => dt.Id == Guid.Parse(DestinationTypes.DockerEngineId) ||
                                                         dt.Id == Guid.Parse(DestinationTypes.PodmanContainerId))
                    .ToList();
            case var _ when resourceTypeId == ResourceTypes.WebAssembly:
                return validDestinationTypes.Where(dt => dt.Id == Guid.Parse(DestinationTypes.WasmtimeId)).ToList();
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceTypeId));
        }
    }
}
