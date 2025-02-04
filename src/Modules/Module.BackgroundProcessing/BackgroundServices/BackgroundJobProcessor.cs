using System.Text.Json;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module.BackgroundProcessing.Features.BackgroundProcessing.ChangeBackgroundJobStatus;
using Module.BackgroundProcessing.Features.BackgroundProcessing.GetNextBackgroundJobGroup;

namespace Module.BackgroundProcessing.BackgroundServices;

public class BackgroundJobProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundJobProcessor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var backgroundJobs = await mediator.Send(new GetNextBackgroundJobGroupQuery(), stoppingToken);

        foreach (var backgroundJob in backgroundJobs)
        {
            var jobType = Type.GetType(backgroundJob.PayloadType);
            if (jobType == null)
            {
                continue;
            }

            object? job = JsonSerializer.Deserialize(backgroundJob.Payload, jobType);
            if (job == null)
            {
                continue;
            }

            await mediator.Send(
                new ChangeBackgroundJobStatusCommand { Id = backgroundJob.Id, Status = BackgroundJobStatus.Running },
                stoppingToken);

            try
            {
                await mediator.Send(job, stoppingToken);
            }
            catch (Exception e)
            {
                await mediator.Send(
                    new ChangeBackgroundJobStatusCommand { Id = backgroundJob.Id, Status = BackgroundJobStatus.Failed },
                    stoppingToken);

                continue;
            }

            await mediator.Send(
                new ChangeBackgroundJobStatusCommand { Id = backgroundJob.Id, Status = BackgroundJobStatus.Completed },
                stoppingToken);
        }
    }
}
