using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Module.Webhook.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddWebhookModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMartenStore<IWebhookStore>(sp =>
        {
            var options = new StoreOptions();
            options.Connection(sp.GetRequiredService<NpgsqlDataSource>());
            options.DatabaseSchemaName = "webhook";

            options.UseSystemTextJsonForSerialization();

            options.Schema.For<Webhook>().Identity(w => w.Id);

            return options;
        }).OptimizeArtifactWorkflow();

        return builder;
    }
}