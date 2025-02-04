using Marten;
using Mediator;

namespace Module.BackgroundProcessing.Features.BackgroundProcessing.GetNextBackgroundJobGroup;

public class
    GetNextBackgroundJobGroupQueryHandler : IQueryHandler<GetNextBackgroundJobGroupQuery, IReadOnlyList<BackgroundJob>>
{
    private readonly IBackgroundProcessingStore _backgroundProcessingStore;

    public GetNextBackgroundJobGroupQueryHandler(IBackgroundProcessingStore backgroundProcessingStore)
    {
        _backgroundProcessingStore = backgroundProcessingStore;
    }

    public async ValueTask<IReadOnlyList<BackgroundJob>> Handle(GetNextBackgroundJobGroupQuery query,
        CancellationToken cancellationToken)
    {
        await using IQuerySession session = _backgroundProcessingStore.QuerySession();

        string? nextGroup = await session.Query<BackgroundJob>()
            .Where(job => job.Status == BackgroundJobStatus.Queued)
            .OrderBy(job => job.CreatedAt)
            .Select(job => job.Group)
            .FirstOrDefaultAsync(cancellationToken);

        if (nextGroup == null)
        {
            return Array.Empty<BackgroundJob>();
        }

        return await session.Query<BackgroundJob>()
            .Where(job => job.Status == BackgroundJobStatus.Queued && job.Group == nextGroup)
            .OrderBy(job => job.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
