using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Features.Deployment.Factories;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public class DeploymentPipelineService(
    [FromKeyedServices("DeploymentSourceServiceFactory")]
    IDeploymentServiceFactory sourceServiceFactory,
    [FromKeyedServices("DeploymentTargetServiceFactory")]
    IDeploymentServiceFactory targetServiceFactory) {
    public async Task<DeploymentContext> InsertContextAsync(DeploymentContext context) {
        var sourceService = sourceServiceFactory.Create(context);
        var targetService = targetServiceFactory.Create(context);

        sourceService.SetNext(targetService);
        return await sourceService.Handle(context);
    }
}