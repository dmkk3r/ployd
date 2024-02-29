using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;
using Ployd.Peasant.Services.Docker;
using Ployd.Peasant.Services.OperatingSystem;

namespace Ployd.Peasant.Features.Deployment.Handlers;

public class DockerfileTarget(IOperatingSystem operatingSystem) : IDeploymentHander {
    public bool CanHandle(CreateDeploymentRequest request) {
        return request is { DeploymentTarget: DeploymentTarget.Dockerfile, TargetParameter: DockerfileParameter };
    }

    public async Task<(IDeploymentResult?, DeploymentError?)> Handle(CreateDeploymentRequest request, IDeploymentResult? handledResult = null) {
        if (request.TargetParameter is not DockerfileParameter dockerfileDeploymentFeature)
            return (null, new DeploymentError("Invalid target parameter"));

        if (handledResult is not DeploymentSourceResult deploymentSourceFeature)
            return (null, new DeploymentError("Invalid source parameter"));

        await CreateImage(deploymentSourceFeature.SourceDirectory, dockerfileDeploymentFeature.BuildContext, dockerfileDeploymentFeature.ImageName);
        await RunContainer(dockerfileDeploymentFeature.ImageName, dockerfileDeploymentFeature.ContainerName, dockerfileDeploymentFeature.PortMapping);

        var deploymentTargetData = new DeploymentTargetResult(dockerfileDeploymentFeature.ContainerName, "");
        return (deploymentTargetData, null);
    }

    private async Task CreateImage(string sourceDirectory, string buildContext, string imageName) {
        var workingDirectory = Path.Combine(sourceDirectory, buildContext);
        var dockerfile = GetDockerfilePath(workingDirectory);

        await DockerCli.Build(workingDirectory, dockerfile, imageName);
    }

    private static async Task RunContainer(string imageName, string containerName, (int outside, int inside) portMapping) {
        await DockerCli.Run(imageName, containerName, portMapping);
    }

    private string GetDockerfilePath(string sourceDirectory) {
        var normalizedSourceDirectory = operatingSystem.NormalizePath(sourceDirectory);
        var dockerfile = Directory.GetFiles(normalizedSourceDirectory, "Dockerfile", SearchOption.AllDirectories).First();
        return dockerfile.Replace(normalizedSourceDirectory, "").TrimStart('\\');
    }
}