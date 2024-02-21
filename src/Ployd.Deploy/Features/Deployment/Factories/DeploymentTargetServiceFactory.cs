using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Features.Deployment.Pipeline;

namespace Ployd.Deploy.Features.Deployment.Factories;

public class DeploymentTargetServiceFactory(IServiceProvider serviceProvider) : IDeploymentServiceFactory {
    public IDeploymentPipelineHander Create(DeploymentContext context) {
        switch (context.TargetType)
        {
            case DeploymentTargetType.Dockerfile:
                context.Features?.Add(new DockerfileDeploymentFeature());
                return serviceProvider.GetRequiredService<DockerfileTargetPipelineHander>();
            case DeploymentTargetType.DockerCompose:
                context.Features?.Add(new DockerComposeDeploymentFeature());
                return serviceProvider.GetRequiredService<DockerComposeTargetPipelineHander>();
            default:
                throw new ArgumentException($"Unsupported target type: {context.TargetType}", nameof(context.TargetType));
        }
    }
}