using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Features.Deployment.Pipeline;

namespace Ployd.Deploy.Features.Deployment.Factories;

public interface IDeploymentServiceFactory {
    IDeploymentPipelineHander Create(DeploymentContext context);
}