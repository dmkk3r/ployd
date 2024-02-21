namespace Ployd.Core.Models.Deployments;

public class CreateDeploymentRequest {
    public required Uri Repository { get; set; }
    public DeploymentSourceType DeploymentSourceType { get; set; }
    public DeploymentTargetType DeploymentTargetType { get; set; }
    public bool Watch { get; set; }
}