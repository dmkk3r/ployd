namespace Ployd.Peasant.Services.BackgroundServices;

public class DeploymentBackgroundQueue : BackgroundService {
    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        return Task.CompletedTask;
    }
}