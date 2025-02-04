using Marten;
using Mediator;

namespace Module.BackgroundProcessing.Features.BackgroundProcessing.ChangeBackgroundJobStatus;

public class ChangeBackgroundJobStatusCommandHandler : ICommandHandler<ChangeBackgroundJobStatusCommand>
{
    private readonly IBackgroundProcessingStore _backgroundProcessingStore;

    public ChangeBackgroundJobStatusCommandHandler(IBackgroundProcessingStore backgroundProcessingStore)
    {
        _backgroundProcessingStore = backgroundProcessingStore;
    }

    public async ValueTask<Unit> Handle(ChangeBackgroundJobStatusCommand command, CancellationToken cancellationToken)
    {
        await using IDocumentSession session = _backgroundProcessingStore.LightweightSession();

        BackgroundJob? backgroundJob = await session.LoadAsync<BackgroundJob>(command.Id, cancellationToken);

        if (backgroundJob is null)
        {
            return Unit.Value;
        }

        switch (command.Status)
        {
            case BackgroundJobStatus.Running:
                backgroundJob.Status = BackgroundJobStatus.Running;
                backgroundJob.StartedAt = DateTimeOffset.UtcNow;
                break;
            case BackgroundJobStatus.Completed:
                backgroundJob.Status = BackgroundJobStatus.Completed;
                backgroundJob.FinishedAt = DateTimeOffset.UtcNow;
                break;
            case BackgroundJobStatus.Failed:
                backgroundJob.Status = BackgroundJobStatus.Failed;
                backgroundJob.FinishedAt = DateTimeOffset.UtcNow;
                break;
            case BackgroundJobStatus.Queued:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        session.Store(backgroundJob);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
