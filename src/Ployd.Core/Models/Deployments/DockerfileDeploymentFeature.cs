namespace Ployd.Core.Models.Deployments;

public class DockerfileDeploymentFeature : IDeploymentFeature {
    public string BuildContext { get; set; }
    public string ImageName { get; set; }
    public string ContainerName { get; set; }
    public (int, int) PortMapping { get; set; }
}