namespace Ployd.Core.Models.Deployments.Results;

public record DeploymentSourceResult(string SourceDirectory) : IDeploymentResult;