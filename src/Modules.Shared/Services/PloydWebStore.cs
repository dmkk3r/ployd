using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Interfaces;

namespace Modules.Shared.Services;

public class PloydWebStore(IHttpContextAccessor httpContextAccessor) : IPloydWebStore
{
    private const string CookiePrefix = "ployd-store";
    private readonly ConcurrentDictionary<string, object?> _perRequestCache = new();
    private readonly ConcurrentBag<string> _perRequestCleared = [];

    public Task StoreAsync<T>(string key, T value)
    {
        _perRequestCache[key] = value;

        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            $"{CookiePrefix}-{key}",
            JsonSerializer.Serialize(value),
            new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });

        return Task.CompletedTask;
    }

    public Task<T?> RetrieveAsync<T>(string key)
    {
        bool cacheValue = _perRequestCache.TryGetValue(key, out object? storeValue);

        if (cacheValue)
        {
            return Task.FromResult((T?)storeValue);
        }

        if (_perRequestCleared.Contains(key))
        {
            return Task.FromResult(default(T));
        }

        return Task.FromResult(httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue($"{CookiePrefix}-{key}",
            out string? cookieValue) == true
            ? JsonSerializer.Deserialize<T>(cookieValue)
            : default);
    }

    public Task ClearAsync(string key)
    {
        _perRequestCache.TryRemove(key, out _);
        _perRequestCleared.Add(key);

        httpContextAccessor.HttpContext?.Response.Cookies.Delete($"{CookiePrefix}-{key}");

        return Task.CompletedTask;
    }
}
