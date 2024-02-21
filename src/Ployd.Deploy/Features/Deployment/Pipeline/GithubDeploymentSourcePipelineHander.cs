using Ployd.Core.Models.Deployments;
using Ployd.Deploy.Services.Github.Repository;

namespace Ployd.Deploy.Features.Deployment.Pipeline;

public class GithubDeploymentSourcePipelineHander(GithubRepositoryService githubRepositoryService) : DeploymentPipelineHandler {
    public override async Task<DeploymentContext> Handle(DeploymentContext context) {
        if (context.SourceType == DeploymentSourceType.Github)
        {
            var githubDeploymentData = context.Features.OfType<GithubDeploymentFeature>().FirstOrDefault();

            if (githubDeploymentData == null) return context;

            var repositoryId = await githubRepositoryService.GetRepositoryIdAsync(githubDeploymentData.Repository);
            var sourcePath = await githubRepositoryService.CloneAsync(repositoryId);

            var deploymentSourceData = context.Features.OfType<DeploymentSourceFeature>().FirstOrDefault();

            if (deploymentSourceData == null) return context;

            deploymentSourceData.SourceDirectory = sourcePath;
        }

        if (Next != null)
        {
            return await Next.Handle(context);
        }

        return context;
    }
}