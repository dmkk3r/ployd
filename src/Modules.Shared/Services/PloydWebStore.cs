using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Interfaces;

namespace Modules.Shared.Services;

public class PloydWebStore : IPloydWebStore
{
    private const string CookiePrefix = "ployd-store";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public PloydWebStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task StoreAsync<T>(string key, T value)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            $"{CookiePrefix}-{key}",
            JsonSerializer.Serialize(value),
            new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });

        return Task.CompletedTask;
    }

    public Task<T?> RetrieveAsync<T>(string key)
    {
        string? value = _httpContextAccessor.HttpContext?.Request.Cookies[$"{CookiePrefix}-{key}"];
        return value == null ? Task.FromResult<T?>(default) : Task.FromResult(JsonSerializer.Deserialize<T>(value));
    }
}
