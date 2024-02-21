using Ployd.Core.Models.Deployments;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public abstract class DeploymentPipelineHandler : IDeploymentPipelineHander {
    protected IDeploymentPipelineHander? Next;

    public void SetNext(IDeploymentPipelineHander? next) {
        Next = next;
    }

    public abstract Task<DeploymentContext> Handle(DeploymentContext context);
}