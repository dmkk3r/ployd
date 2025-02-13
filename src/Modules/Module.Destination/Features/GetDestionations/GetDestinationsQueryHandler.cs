using Mediator;
using Module.Destination.Contract;

namespace Module.Destination.Features.GetDestionations;

public class GetDestinationsQueryHandler : IQueryHandler<GetDestinationsQuery, IReadOnlyList<Contract.Destination>>
{
    public ValueTask<IReadOnlyList<Contract.Destination>> Handle(GetDestinationsQuery query, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
