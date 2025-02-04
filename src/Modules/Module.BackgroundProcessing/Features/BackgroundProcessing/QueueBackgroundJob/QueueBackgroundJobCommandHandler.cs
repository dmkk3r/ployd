using Marten;
using Mediator;
using Module.BackgroundProcessing.Contract;

namespace Module.BackgroundProcessing.Features.BackgroundProcessing.QueueBackgroundJob;

public class QueueBackgroundJobCommandHandler : ICommandHandler<QueueBackgroundJobCommand>
{
    private readonly IBackgroundProcessingStore _backgroundProcessingStore;

    public QueueBackgroundJobCommandHandler(IBackgroundProcessingStore backgroundProcessingStore)
    {
        _backgroundProcessingStore = backgroundProcessingStore;
    }

    public async ValueTask<Unit> Handle(QueueBackgroundJobCommand command, CancellationToken cancellationToken)
    {
        await using IDocumentSession session = _backgroundProcessingStore.LightweightSession();

        BackgroundJob backgroundJob = new()
        {
            CreatedAt = DateTimeOffset.UtcNow,
            Status = BackgroundJobStatus.Queued,
            Group = command.Group,
            PayloadType = command.PayloadType.AssemblyQualifiedName!,
            Payload = command.Payload
        };

        session.Store(backgroundJob);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
