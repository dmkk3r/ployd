namespace Ployd.Core.Models.Deployments.Results;

public record DeploymentTargetResult(string Name, string Endpoint) : IDeploymentResult;