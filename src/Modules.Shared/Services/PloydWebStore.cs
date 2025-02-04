using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Modules.Shared.Interfaces;

namespace Modules.Shared.Services;

public class PloydWebStore(IHttpContextAccessor httpContextAccessor) : IPloydWebStore
{
    private const string CookiePrefix = "ployd-store";
    private readonly ConcurrentDictionary<string, object?> _perRequestCache = new();
    private readonly ConcurrentBag<string> _markedForDeletion = [];

    public Task StoreAsync<T>(string key, T value)
    {
        _perRequestCache[key] = value;

        var type = value?.GetType();

        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            $"{CookiePrefix}-{key}",
            JsonSerializer.Serialize(value, type),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddDays(1)
            });

        return Task.CompletedTask;
    }

    public Task<T?> RetrieveAsync<T>(string key)
    {
        bool cacheValue = _perRequestCache.TryGetValue(key, out object? storeValue);

        if (cacheValue)
        {
            return Task.FromResult((T?)storeValue);
        }

        if (_markedForDeletion.Contains(key))
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
        _markedForDeletion.Add(key);
        httpContextAccessor.HttpContext?.Response.Cookies.Delete($"{CookiePrefix}-{key}",
            new CookieOptions { SameSite = SameSiteMode.Lax });

        return Task.CompletedTask;
    }
}
