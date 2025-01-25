namespace Modules.Shared.Interfaces;

public interface IPloydWebStore
{
    Task StoreAsync<T>(string key, T value);
    Task<T?> RetrieveAsync<T>(string key);
}
