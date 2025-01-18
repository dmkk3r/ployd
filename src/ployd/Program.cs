using Marten;
using Module.Webhook.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string not found."));

builder.Services.AddMarten()
    .UseLightweightSessions()
    .UseNpgsqlDataSource();

builder.Services.AddMediator();

builder.AddWebhookModule();

var app = builder.Build();

app.MapWebhookModule();

app.Run();