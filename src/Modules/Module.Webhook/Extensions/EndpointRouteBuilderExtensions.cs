using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Module.Webhook.Features.CreateWebhook;

namespace Module.Webhook.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapWebhookModule(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/webhooks", PostWebhooksHandle);
        return builder;
    }

    private static async Task<IResult> PostWebhooksHandle(HttpContext context, CancellationToken cancellationToken)
    {
        var mediator = context.RequestServices.GetRequiredService<IMediator>();

        await mediator.Send(new CreateWebhookCommand(), cancellationToken);

        return Results.Created("/webhooks", null);
    }
}