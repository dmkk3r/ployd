using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Ployd.Cli.Services;
using Ployd.Cli.Services.Deployments;
using Ployd.Core.Models.Deployments;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddScoped<DeploymentService>();

builder.Services.AddHttpClient("ployd-deployments", c => {
    c.BaseAddress = new Uri("http://localhost:5275");
});

var app = builder.Build();

app.AddSubCommand("deployment", c => {
    c.AddCommand("create", async (
        [Option(Description = "The repository url to deploy")]
        Uri repository,
        [Option(Description = "The source type to deploy")]
        DeploymentSource? sourceType,
        [Option(Description = "The target type to deploy")]
        DeploymentTarget? targetType,
        [Option(Description = "Watch the repository for changes")]
        bool? watch,
        [FromService] DeploymentService deploymentService
    ) => {
        await deploymentService.DeployAsync(repository, sourceType, targetType, watch);
    }).WithDescription("Create a new deployment");

    c.AddCommand("delete", async (
        [Argument(Description = "The deployment id to delete")]
        Guid id,
        [FromService] DeploymentService deploymentService
    ) => {
        await deploymentService.DeleteAsync(id);
    }).WithDescription("Delete a deployment");

    c.AddCommand("update", async (
        [Argument(Description = "The deployment id to delete")]
        Guid id,
        [FromService] DeploymentService deploymentService
    ) => {
        await deploymentService.UpdateAsync(id);
    }).WithDescription("Update a deployment");

    c.AddCommand("list", async (
        [FromService] DeploymentService deploymentService) => {
        await deploymentService.ListAsync();
    }).WithDescription("List all deployments");

    c.AddCommand("start", async (
        [Argument(Description = "The deployment id to delete")]
        Guid id,
        [FromService] DeploymentService deploymentService
    ) => {
        await deploymentService.StartAsync(id);
    }).WithDescription("Start a deployment");

    c.AddCommand("stop", async (
        [Argument(Description = "The deployment id to delete")]
        Guid id,
        [FromService] DeploymentService deploymentService
    ) => {
        await deploymentService.StopAsync(id);
    }).WithDescription("Stop a deployment");
});

app.Run();