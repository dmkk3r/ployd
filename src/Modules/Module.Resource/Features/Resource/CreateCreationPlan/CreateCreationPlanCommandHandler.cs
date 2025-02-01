using Mediator;

namespace Module.Resource.Features.Resource.CreateCreationPlan;

public class CreateCreationPlanCommandHandler : IRequestHandler<CreateCreationPlanCommand>
{
    public ValueTask<Unit> Handle(CreateCreationPlanCommand request, CancellationToken cancellationToken)
    {
        return new ValueTask<Unit>(Unit.Value);
    }
}
