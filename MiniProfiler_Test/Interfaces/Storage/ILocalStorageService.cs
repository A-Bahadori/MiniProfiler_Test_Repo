namespace MiniProfiler_Test.Interfaces.Storage;

public interface ILocalStorageService
{
    void ClearStorage(List<string> keys);
    bool Exists(string key);
    T GetStorageValue<T>(string key);
    void SetStorageValue<T>(string key, T value);
    void SetStorageValueWithExpiry<T>(string key, T value, TimeSpan expiry);
    T GetStorageValueWithExpiry<T>(string key);
}