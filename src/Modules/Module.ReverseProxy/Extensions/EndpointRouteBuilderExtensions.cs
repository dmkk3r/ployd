using Microsoft.AspNetCore.Routing;

namespace Module.ReverseProxy.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapReverseProxyModule(this IEndpointRouteBuilder builder) => builder;
}
