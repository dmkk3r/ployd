using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Core.Services;
using Ployd.Peasant.Features.Deployment.Handlers;
using Ployd.Silo;
using Ployd.Silo.Entities;

namespace Ployd.Peasant.Features.Deployment;

public class DeploymentService(
    ILogger<DeploymentService> logger,
    PloydStoreContext context,
    DeploymentCoordinator deploymentCoordinator) {
    public async Task<Core.Models.Deployments.Deployment> CreateDeploymentAsync(CreateDeploymentRequest request) {
        var deploymentSource = new DeploymentSourceEntity
        {
            Id = Guid.NewGuid(),
            Name = ResourceNameGenerator.GenerateResourceName(),
            DeploymentSource = request.DeploymentSource
        };

        var deploymentTarget = new DeploymentTargetEntity
        {
            Id = Guid.NewGuid(),
            Name = ResourceNameGenerator.GenerateResourceName(),
            DeploymentTarget = request.DeploymentTarget
        };

        var deployment = new DeploymentEntity
        {
            Id = Guid.NewGuid(),
            Name = ResourceNameGenerator.GenerateResourceName(),
            Status = DeploymentStatus.Undeployed,
            DeploymentSource = deploymentSource,
            DeploymentTargets = new List<DeploymentTargetEntity> { deploymentTarget }
        };

        context.Deployments.Add(deployment);
        await context.SaveChangesAsync();

        deploymentCoordinator
            .WithSource<GithubDeploymentSource>()
            .WithTarget<DockerfileTarget>()
            .WithTarget<DockerComposeTarget>();

        await deploymentCoordinator.Deploy(request);

        return new Core.Models.Deployments.Deployment
        {
            Id = deployment.Id,
            Name = deployment.Name,
            Status = deployment.Status,
        };
    }

    public Task DeleteAsync(Guid id) {
        logger.LogInformation("");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(UpdateDeploymentRequest request) {
        logger.LogInformation("");
        return Task.CompletedTask;
    }

    public Task StartAsync(StartDeploymentRequest request) {
        logger.LogInformation("");
        return Task.CompletedTask;
    }

    public Task StopAsync(StopDeploymentRequest request) {
        logger.LogInformation("");
        return Task.CompletedTask;
    }

    public Task<List<Core.Models.Deployments.Deployment>> ListAsync() {
        logger.LogInformation("");
        return Task.FromResult<List<Core.Models.Deployments.Deployment>>([]);
    }
}