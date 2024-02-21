using Ployd.Core.Models.Deployments;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public class DockerComposeTargetPipelineHander : DeploymentPipelineHandler {
    public override async Task<DeploymentContext> Handle(DeploymentContext context) {
        if (context.TargetType == DeploymentTargetType.DockerCompose)
        {
            return new DeploymentContext();
        }
        else if (Next != null)
        {
            return await Next.Handle(context);
        }
        else
        {
            throw new Exception("No matching service found");
        }
    }
}