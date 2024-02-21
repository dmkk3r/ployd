using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Services.Docker;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public class DockerfileTargetPipelineHander : DeploymentPipelineHandler {
    public override async Task<DeploymentContext> Handle(DeploymentContext context) {
        if (context.TargetType == DeploymentTargetType.Dockerfile)
        {
            var dockerfileDeploymentData = context.Features.OfType<DockerfileDeploymentFeature>().FirstOrDefault();
            var deploymentSourceData = context.Features.OfType<DeploymentSourceFeature>().FirstOrDefault();

            if (dockerfileDeploymentData == null) return context;
            if (deploymentSourceData == null) return context;

            await CreateImage(deploymentSourceData.SourceDirectory, dockerfileDeploymentData.BuildContext, dockerfileDeploymentData.ImageName);
            await RunContainer(dockerfileDeploymentData.ImageName, dockerfileDeploymentData.ContainerName, dockerfileDeploymentData.PortMapping);
        }

        if (Next != null)
        {
            return await Next.Handle(context);
        }

        return context;
    }

    private static async Task CreateImage(string sourceDirectory, string buildContext, string imageName) {
        var workingDirectory = Path.Combine(sourceDirectory, buildContext);
        var dockerfile = GetDockerfilePath(workingDirectory);

        await DockerCli.Build(workingDirectory, dockerfile, imageName);
    }

    private static async Task RunContainer(string imageName, string containerName, (int outside, int inside) portMapping) {
        await DockerCli.Run(imageName, containerName, portMapping);
    }

    private static string GetDockerfilePath(string sourceDirectory) {
        var normalizedSourceDirectory = NormalizePath(sourceDirectory);
        var dockerfile = Directory.GetFiles(normalizedSourceDirectory, "Dockerfile", SearchOption.AllDirectories).First();
        return dockerfile.Replace(normalizedSourceDirectory, "").TrimStart('\\');
    }

    private static string NormalizePath(string path) {
        return path.Replace('/', '\\');
    }
}