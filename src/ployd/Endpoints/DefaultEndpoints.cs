using Modules.Ui.Layouts;
using RazorHx.Results;

namespace ployd.Endpoints;

public static class DefaultEndpoints
{
    public static IEndpointRouteBuilder MapDefault(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/",
            () => new RazorHxResult<RootLayout>(new { Version = ThisAssembly.Info.InformationalVersion[..15], }));

        return builder;
    }
}
