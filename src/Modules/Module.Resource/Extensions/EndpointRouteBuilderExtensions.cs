using Microsoft.AspNetCore.Routing;

namespace Module.Resource.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapResourceModule(this IEndpointRouteBuilder builder) => builder;
}
