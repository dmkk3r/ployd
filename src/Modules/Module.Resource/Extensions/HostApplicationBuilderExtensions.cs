using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Module.Resource.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddResourceModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMartenStore<IResourceStore>(sp =>
        {
            StoreOptions options = new();
            options.Connection(sp.GetRequiredService<NpgsqlDataSource>());
            options.DatabaseSchemaName = "resource";

            options.UseSystemTextJsonForSerialization();

            return options;
        }).OptimizeArtifactWorkflow();

        return builder;
    }
}
