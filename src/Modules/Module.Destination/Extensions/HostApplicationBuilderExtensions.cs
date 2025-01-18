using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Module.Destination.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddDestinationModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMartenStore<IDestinationStore>(sp =>
        {
            StoreOptions options = new();
            options.Connection(sp.GetRequiredService<NpgsqlDataSource>());
            options.DatabaseSchemaName = "destination";

            options.Schema.For<Destination>()
                .Identity(r => r.Id)
                .AddSubClass<DockerDestination>();

            options.UseSystemTextJsonForSerialization(configure: settings =>
            {
                settings.AllowOutOfOrderMetadataProperties = true;
            });

            return options;
        }).OptimizeArtifactWorkflow();

        return builder;
    }
}
