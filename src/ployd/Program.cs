using Marten;
using Module.Destination.Extensions;
using Module.Resource.Extensions;
using Module.ReverseProxy.Extensions;
using Module.Webhook.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string not found."));

builder.Services.AddMarten()
    .UseLightweightSessions()
    .UseNpgsqlDataSource();

builder.Services.AddMediator();

builder.AddResourceModule();
builder.AddWebhookModule();
builder.AddReverseProxyModule();
builder.AddDestinationModule();

WebApplication app = builder.Build();

app.MapResourceModule();
app.MapWebhookModule();
app.MapReverseProxyModule();
app.MapDestinationModule();

app.Run();
