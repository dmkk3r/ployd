namespace Ployd.Core.Models.Deployments;

public class DeploymentSourceFeature : IDeploymentFeature {
    public Uri Repository { get; set; }
    public bool Watch { get; set; }
    public string SourceDirectory { get; set; }
}