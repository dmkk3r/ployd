namespace Ployd.Core.Models.Deployments.Parameter;

public class DockerComposeParameter : DeploymentParameter {
    public string ComposeFile { get; set; }
}