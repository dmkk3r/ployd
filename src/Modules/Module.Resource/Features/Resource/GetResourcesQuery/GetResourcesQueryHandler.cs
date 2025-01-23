using Marten;
using Mediator;

namespace Module.Resource.Features.Resource.GetResourcesQuery;

public class GetResourcesQueryHandler(IResourceStore store)
    : IRequestHandler<GetResourcesQuery, IReadOnlyList<Module.Resource.Resource>>
{
    public async ValueTask<IReadOnlyList<Module.Resource.Resource>> Handle(GetResourcesQuery request,
        CancellationToken cancellationToken)
    {
        await using IQuerySession session = store.QuerySession();

        return await session.Query<Module.Resource.Resource>().ToListAsync(cancellationToken);
    }
}
