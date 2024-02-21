using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Models.Deployments.Results;
using Ployd.Deploy.Services.Github.Repository;

namespace Ployd.Deploy.Features.Deployment.Handlers;

public class GithubDeploymentSource(GithubRepositoryService githubRepositoryService) : IDeploymentHander {
    public bool CanHandle(CreateDeploymentRequest request) {
        return request is { DeploymentSource: DeploymentSource.Github, SourceParameter: GithubParameter };
    }

    public async Task<(IDeploymentResult?, DeploymentError?)> Handle(CreateDeploymentRequest request, IDeploymentResult? handledResult = null) {
        if (request.SourceParameter is not GithubParameter githubDeploymentFeature)
            return (null, new DeploymentError("Invalid target parameter"));

        var repositoryId = await githubRepositoryService.GetRepositoryIdAsync(githubDeploymentFeature.Repository);
        var sourcePath = await githubRepositoryService.CloneAsync(repositoryId);

        var deploymentSourceData = new DeploymentSourceResult(sourcePath);
        return (deploymentSourceData, null);
    }
}