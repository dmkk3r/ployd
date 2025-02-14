using Marten;
using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.GetDestionations;

public class GetDestinationsQueryHandler(IDestinationStore store) : IQueryHandler<GetDestinationsQuery, IReadOnlyList<Contract.Destination>>
{
    public async ValueTask<IReadOnlyList<Contract.Destination>> Handle(GetDestinationsQuery query,
        CancellationToken cancellationToken)
    {
        await using IQuerySession session = store.QuerySession();

        return await session.Query<Contract.Destination>().ToListAsync(cancellationToken);
    }
}
