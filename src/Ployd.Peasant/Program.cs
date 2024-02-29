using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Ployd.Core.Models.Deployments.Requests;
using Ployd.Peasant.Features.Deployment;
using Ployd.Peasant.Features.Deployment.Handlers;
using Ployd.Peasant.Models.Github;
using Ployd.Peasant.Services.Github;
using Ployd.Peasant.Services.Github.Repository;
using Ployd.Peasant.Services.Github.Webhook;
using Ployd.Peasant.Services.OperatingSystem;
using Ployd.Silo.Extensions;
using DeploymentService = Ployd.Peasant.Features.Deployment.DeploymentService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IGithubClientFactory, GithubClientFactory>();
builder.Services.AddScoped<GithubWebhookService>();
builder.Services.AddScoped<GithubRepositoryService>();

builder.Services.AddScoped<DeploymentService>();
builder.Services.AddScoped<DeploymentCoordinator>();
builder.Services.AddScoped<GithubDeploymentSource>();
builder.Services.AddScoped<DockerfileTarget>();
builder.Services.AddScoped<DockerComposeTarget>();

builder.Services.AddScoped<IOperatingSystem>(_ =>
    Environment.OSVersion.Platform == PlatformID.Win32NT ? new Windows() : new Unix());

builder.Services.AddSqlite(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var deploymentRoutes = app.MapGroup("deployments");

deploymentRoutes.MapPost("create", async (
        CreateDeploymentRequest request,
        [FromServices] DeploymentService deploymentService) => await deploymentService.CreateDeploymentAsync(request))
    .WithOpenApi(operation => new OpenApiOperation(operation)
    {
        Summary = "Create a deployment",
        Description = "Create a deployment",
        OperationId = "CreateDeployment"
    });

deploymentRoutes.MapDelete("{id:guid}/delete", async (
        Guid id,
        [FromServices] DeploymentService deploymentService) => {
        await deploymentService.DeleteAsync(id);
    })
    .WithOpenApi(operation => new OpenApiOperation(operation)
    {
        Summary = "Delete a deployment",
        Description = "Delete a deployment",
        OperationId = "DeleteDeployment"
    });

deploymentRoutes.MapPut("{id:guid}/update", async (
    UpdateDeploymentRequest request,
    [FromServices] DeploymentService deploymentService) => {
    await deploymentService.UpdateAsync(request);
}).WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Update a deployment",
    Description = "Update a deployment",
    OperationId = "UpdateDeployment"
});

deploymentRoutes.MapPut("{id:guid}/start", async (
    StartDeploymentRequest request,
    [FromServices] DeploymentService deploymentService) => {
    await deploymentService.StartAsync(request);
}).WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Start a deployment",
    Description = "Start a deployment",
    OperationId = "StartDeployment"
});

deploymentRoutes.MapPut("{id:guid}/stop", async (
    StopDeploymentRequest request,
    [FromServices] DeploymentService deploymentService) => {
    await deploymentService.StopAsync(request);
}).WithOpenApi(operation => new OpenApiOperation(operation)
{
    Summary = "Stop a deployment",
    Description = "Stop a deployment",
    OperationId = "StopDeployment"
});

deploymentRoutes.MapGet("list", async (
        [FromServices] DeploymentService deploymentService) => await deploymentService.ListAsync())
    .WithOpenApi(operation => new OpenApiOperation(operation)
    {
        Summary = "List deployments",
        Description = "List deployments",
        OperationId = "ListDeployments"
    });

app.MapPost("webhooks/github/{repositoryId:long}/{webhookId:int}", async (
    long repositoryId,
    int webhookId,
    GithubWebhookResponse response,
    [FromServices] GithubWebhookService webhookService) => {
    await webhookService.ConsumeWebhookAsync(repositoryId, webhookId, response);
}).ExcludeFromDescription();

app.Run();