using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;
using Ployd.Deploy.Services.Docker;
using Ployd.Deploy.Services.OperatingSystem;

namespace Ployd.Deploy.Features.Deployment.Handlers;

public class DockerComposeTarget(IOperatingSystem operatingSystem) : IDeploymentHander {
    public bool CanHandle(CreateDeploymentRequest request) {
        return request is { DeploymentTarget: DeploymentTarget.DockerCompose, TargetParameter: DockerComposeParameter };
    }

    public async Task<(IDeploymentResult?, DeploymentError?)> Handle(CreateDeploymentRequest request, IDeploymentResult? handledResult = null) {
        if (request.TargetParameter is not DockerComposeParameter dockerComposeParameter)
            return (null, new DeploymentError("Invalid target parameter"));

        if (handledResult is not DeploymentSourceResult deploymentSourceFeature)
            return (null, new DeploymentError("Invalid source parameter"));

        var workingDirectory = operatingSystem.NormalizePath(deploymentSourceFeature.SourceDirectory);
        
        await DockerCli.ComposeBuild(workingDirectory, dockerComposeParameter.ComposeFile);
        await DockerCli.ComposeUp(workingDirectory, dockerComposeParameter.ComposeFile);
        
        return (null, null);
    }
}