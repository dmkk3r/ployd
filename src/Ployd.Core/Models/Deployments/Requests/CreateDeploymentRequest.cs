using Ployd.Core.Models.Deployments.Parameter;

namespace Ployd.Core.Models.Deployments.Requests;

public class CreateDeploymentRequest {
    public DeploymentSource DeploymentSource { get; set; }
    public DeploymentTarget DeploymentTarget { get; set; }
    public DeploymentParameter? SourceParameter { get; set; }
    public DeploymentParameter? TargetParameter { get; set; }
}