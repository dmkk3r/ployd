namespace Ployd.Core.Models.Deployments;

public class DeploymentContext {
    public DeploymentSourceType SourceType { get; set; }
    public DeploymentTargetType TargetType { get; set; }
    public ICollection<IDeploymentFeature> Features { get; set; } = new List<IDeploymentFeature>();
}