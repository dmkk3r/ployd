using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Module.ReverseProxy.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddReverseProxyModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMartenStore<IReverseProxyStore>(sp =>
        {
            StoreOptions options = new();
            options.Connection(sp.GetRequiredService<NpgsqlDataSource>());
            options.DatabaseSchemaName = "reverseproxy";

            options.Schema.For<ReverseProxyConfiguration>().Identity(rp => rp.Id);

            options.UseSystemTextJsonForSerialization();

            return options;
        }).OptimizeArtifactWorkflow();

        ServiceProvider services = builder.Services.BuildServiceProvider();
        IDocumentSession session = services.GetRequiredService<IReverseProxyStore>().LightweightSession();
        ReverseProxyConfiguration reverseProxyConfiguration =
            session.Query<ReverseProxyConfiguration>().FirstOrDefault()
            ?? new ReverseProxyConfiguration();

        builder.Services.AddReverseProxy()
            .LoadFromMemory(reverseProxyConfiguration.Routes, reverseProxyConfiguration.Clusters);

        return builder;
    }
}
