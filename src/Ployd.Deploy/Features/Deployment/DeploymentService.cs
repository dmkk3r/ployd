using Ployd.Core.Models.Deployments;
using Ployd.Core.Services;
using Ployd.Deploy.Features.Deployment.Pipeline;
using Ployd.Store;
using Ployd.Store.Entities;

namespace Ployd.Deploy.Features.Deployment;

public class DeploymentService(
    ILogger<DeploymentService> logger,
    PloydStoreContext context,
    DeploymentPipelineService pipelineService) {
    public async Task<Core.Models.Deployments.Deployment> CreateDeploymentAsync(CreateDeploymentRequest request) {
        var deploymentSource = new DeploymentSourceEntity
        {
            Id = Guid.NewGuid(),
            Name = ResourceNameGenerator.GenerateResourceName(),
            DeploymentSourceType = request.DeploymentSourceType
        };

        var deploymentTarget = new DeploymentTargetEntity
        {
            Id = Guid.NewGuid(),
            Name = ResourceNameGenerator.GenerateResourceName(),
            DeploymentTargetType = request.DeploymentTargetType
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

        await pipelineService.InsertContextAsync(new DeploymentContext
        {
            SourceType = request.DeploymentSourceType,
            TargetType = request.DeploymentTargetType,
            Features = new List<IDeploymentFeature>
            {
                new DeploymentSourceFeature { Repository = request.Repository, Watch = request.Watch }
            }
        });

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