using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Interfaces;

namespace Modules.Shared.Services;

public class PloydWebStore : IPloydWebStore
{
    private const string CookiePrefix = "ployd-store";
    private readonly ConcurrentDictionary<string, object?> _store = new();

    private readonly IHttpContextAccessor _httpContextAccessor;

    public PloydWebStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task StoreAsync<T>(string key, T value)
    {
        _store[key] = value;

        _httpContextAccessor.HttpContext?.Response.Cookies.Delete($"{CookiePrefix}-{key}");
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            $"{CookiePrefix}-{key}",
            JsonSerializer.Serialize(value),
            new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });

        return Task.CompletedTask;
    }

    public Task<T?> RetrieveAsync<T>(string key)
    {
        bool fromStore = _store.TryGetValue(key, out object? storeValue);

        if (fromStore)
        {
            return Task.FromResult((T?)storeValue);
        }

        return Task.FromResult(_httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue($"{CookiePrefix}-{key}",
            out string? cookieValue) == true
            ? JsonSerializer.Deserialize<T>(cookieValue)
            : default);
    }
}
