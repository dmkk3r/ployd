using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Ployd.Core.Models.Deployments;
using Ployd.Core.Models.Deployments.Parameter;
using Ployd.Core.Models.Deployments.Requests;

namespace Ployd.Cli.Services.Deployments;

public class DeploymentService(IHttpClientFactory httpClientFactory, ILogger<DeploymentService> logger) {
    public async Task DeployAsync(Uri repository, DeploymentSource? sourceType, DeploymentTarget? targetType, bool? watch) {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deploymentCreatedResponse = await client.PostAsJsonAsync("deployments/create", new CreateDeploymentRequest
        {
            DeploymentSource = sourceType ?? DeploymentSource.Github,
            DeploymentTarget = targetType ?? DeploymentTarget.Dockerfile,
            SourceParameter = new GithubParameter
            {
                Repository = repository,
                Branch = "main",
                Watch = watch ?? false
            },
            // TargetParameter = new DockerfileParameter
            // {
            //     BuildContext = "src\\backend",
            //     ImageName = "ployd",
            //     ContainerName = "ployd",
            //     PortMapping = (5080, 8080)
            // },
            TargetParameter = new DockerComposeParameter()
            {
                ComposeFile = "docker-compose.yaml"
            }
        });

        deploymentCreatedResponse.EnsureSuccessStatusCode();

        var deployment = await deploymentCreatedResponse.Content.ReadFromJsonAsync<Deployment>();
        if (deployment != null) logger.LogInformation("Deployment {Deployment} created", deployment);
    }

    public async Task DeleteAsync(Guid id) {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deploymentDeletedResponse = await client.DeleteAsync($"deployments/{id}/delete");

        deploymentDeletedResponse.EnsureSuccessStatusCode();

        logger.LogInformation("Deployment {DeploymentId} deleted", id);
    }

    public async Task UpdateAsync(Guid id) {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deploymentUpdatedResponse = await client.PutAsJsonAsync($"deployments/{id}/update", new UpdateDeploymentRequest
        {
            Id = id
        });

        deploymentUpdatedResponse.EnsureSuccessStatusCode();

        logger.LogInformation("Deployment {DeploymentId} updated", id);
    }

    public async Task ListAsync() {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deployments = await client.GetFromJsonAsync<List<Deployment>>("deployments/list");

        if (deployments != null)
        {
            logger.LogInformation("Deploments: {Deployments}", deployments);
        }
    }

    public async Task StartAsync(Guid id) {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deploymentStartedResponse = await client.PutAsJsonAsync($"deployments/{id}/start", new StartDeploymentRequest
        {
            Id = id
        });

        deploymentStartedResponse.EnsureSuccessStatusCode();

        logger.LogInformation("Deployment {DeploymentId} started", id);
    }

    public async Task StopAsync(Guid id) {
        using var client = httpClientFactory.CreateClient("ployd-deployments");

        var deploymentStoppedResponse = await client.PutAsJsonAsync($"deployments/{id}/stop", new StopDeploymentRequest
        {
            Id = id
        });

        deploymentStoppedResponse.EnsureSuccessStatusCode();

        logger.LogInformation("Deployment {DeploymentId} stopped", id);
    }
}