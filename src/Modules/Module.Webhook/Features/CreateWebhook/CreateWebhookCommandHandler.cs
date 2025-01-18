using Marten;
using Mediator;

namespace Module.Webhook.Features.CreateWebhook;

public class CreateWebhookCommandHandler(IWebhookStore webhookStore) : IRequestHandler<CreateWebhookCommand>
{
    public async ValueTask<Unit> Handle(CreateWebhookCommand request, CancellationToken cancellationToken)
    {
        await using IDocumentSession? session = webhookStore.LightweightSession();

        Webhook? webhook = new() { Id = Guid.NewGuid() };

        session.Store(webhook);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
