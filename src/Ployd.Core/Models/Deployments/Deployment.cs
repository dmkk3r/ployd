namespace Ployd.Core.Models.Deployments;

public class Deployment {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Uri Url { get; set; }
    public DeploymentStatus Status { get; set; }
}