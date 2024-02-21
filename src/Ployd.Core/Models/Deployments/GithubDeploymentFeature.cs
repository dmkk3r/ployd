namespace Ployd.Core.Models.Deployments;

public class GithubDeploymentFeature : IDeploymentFeature {
    public Uri Repository { get; set; }
    public bool Watch { get; set; }
}