using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module.BackgroundProcessing.BackgroundServices;
using Npgsql;

namespace Module.BackgroundProcessing.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddBackgroundProcessingModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMartenStore<IBackgroundProcessingStore>(sp =>
        {
            StoreOptions options = new();
            options.Connection(sp.GetRequiredService<NpgsqlDataSource>());
            options.DatabaseSchemaName = "background_processing";

            options.Schema.For<BackgroundJob>()
                .Identity(r => r.Id);

            options.UseSystemTextJsonForSerialization(configure: settings =>
            {
                settings.AllowOutOfOrderMetadataProperties = true;
            });

            return options;
        }).OptimizeArtifactWorkflow();

        builder.Services.AddHostedService<BackgroundJobProcessor>();

        return builder;
    }
}
