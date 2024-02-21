namespace Ployd.Core.Models.Deployments.Parameter;

public class GithubParameter : DeploymentParameter {
    public Uri Repository { get; set; }
    public string Branch { get; set; }
    public bool Watch { get; set; }
}