using Mediator;

namespace Module.Webhook.Features.CreateWebhook;

public class CreateWebhookCommandHandler(IWebhookStore webhookStore) : IRequestHandler<CreateWebhookCommand>
{
    public async ValueTask<Unit> Handle(CreateWebhookCommand request, CancellationToken cancellationToken)
    {
        await using var session = webhookStore.LightweightSession();

        var webhook = new Webhook
        {
            Id = Guid.NewGuid()
        };

        session.Store(webhook);
        await session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}