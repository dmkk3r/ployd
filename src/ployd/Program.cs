using Marten;
using Module.BackgroundProcessing.Extensions;
using Module.Destination.Extensions;
using Module.Resource.Extensions;
using Module.ReverseProxy.Extensions;
using Module.Webhook.Extensions;
using Modules.Shared.Interfaces;
using Modules.Shared.Services;
using ployd.Endpoints;
using RazorHx.Builder;
using RazorHx.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string not found."));

builder.Services.AddMarten()
    .UseLightweightSessions()
    .UseNpgsqlDataSource();

builder.Services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; });

builder.AddResourceModule();
builder.AddWebhookModule();
builder.AddReverseProxyModule();
builder.AddDestinationModule();
builder.AddBackgroundProcessingModule();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPloydWebStore, PloydWebStore>();
builder.Services.AddSingleton<IVersionInfo>(new VersionInfo { Version = ThisAssembly.AssemblyFileVersion });

builder.Services.AddRazorHxComponents(options => { options.RootComponent = typeof(Modules.Ui.Index); });

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRazorHxComponents();

app.MapDefault();
app.MapResourceModule();
app.MapWebhookModule();
app.MapReverseProxyModule();
app.MapDestinationModule();

app.Run();
