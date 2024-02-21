using Ployd.Core.Models.Deployments;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public interface IDeploymentPipelineHander {
    void SetNext(IDeploymentPipelineHander? next);
    Task<DeploymentContext> Handle(DeploymentContext context);
}