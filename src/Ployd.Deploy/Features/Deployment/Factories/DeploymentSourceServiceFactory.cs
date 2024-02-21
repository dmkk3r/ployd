using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Features.Deployment.Pipeline;

namespace Ployd.Deploy.Features.Deployment.Factories;

public class DeploymentSourceServiceFactory(IServiceProvider serviceProvider) : IDeploymentServiceFactory {
    public IDeploymentPipelineHander Create(DeploymentContext context) {
        var deploymentSourceData = context.Features.OfType<DeploymentSourceFeature>().FirstOrDefault();

        switch (context.SourceType)
        {
            case DeploymentSourceType.Github:
                context.Features.Add(new GithubDeploymentFeature
                {
                    Repository = deploymentSourceData.Repository,
                    Watch = deploymentSourceData.Watch
                });

                return serviceProvider.GetRequiredService<GithubDeploymentSourcePipelineHander>();
            default:
                throw new ArgumentException($"Unsupported source type: {context.SourceType}", nameof(context.SourceType));
        }
    }
}