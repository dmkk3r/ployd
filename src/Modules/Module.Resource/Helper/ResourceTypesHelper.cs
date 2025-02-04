using Module.Resource.Contract;
using Module.Source.Contract;

namespace Module.Resource.Helper;

public class ResourceTypesHelper
{
    public static List<ResourceTypeUi> ValidResourceTypes(Guid? sourceId, string? group = null)
    {
        List<ResourceTypeUi> validResourceTypes;

        if (group == null)
        {
            validResourceTypes = ResourceTypesUi.AllGrouped()
                .SelectMany(rt => rt.Value).ToList();
        }
        else
        {
            validResourceTypes = ResourceTypesUi.AllGrouped().Where(rt => rt.Key == group)
                .SelectMany(rt => rt.Value).ToList();
        }

        switch (sourceId)
        {
            case var _ when sourceId == SourceTypes.Git:
            case var _ when sourceId == SourceTypes.GitHub:
            case var _ when sourceId == SourceTypes.GitLab:
                return validResourceTypes.Where(rt => rt.Id == Guid.Parse(ResourceTypes.ContainerId) ||
                                                      rt.Id == Guid.Parse(ResourceTypes.DockerComposeId) ||
                                                      rt.Id == Guid.Parse(ResourceTypes.PodmanComposeId) ||
                                                      rt.Id == Guid.Parse(ResourceTypes.WebAssemblyId)).ToList();
            case var _ when sourceId == SourceTypes.DockerHub:
            case var _ when sourceId == SourceTypes.Ghcr:
                return validResourceTypes.Where(rt => rt.Id == Guid.Parse(ResourceTypes.ContainerId)).ToList();
            default:
                throw new ArgumentOutOfRangeException(nameof(sourceId));
        }
    }
}
