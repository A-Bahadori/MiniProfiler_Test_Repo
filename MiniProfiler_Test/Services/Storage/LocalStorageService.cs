using Hanssens.Net;
using System.Text.Json;
using MiniProfiler_Test.Interfaces.Storage;
using MiniProfiler_Test.Models.Storage;

namespace MiniProfiler_Test.Services.Storage;

public class LocalStorageService : ILocalStorageService
{
    #region Constractor
    private LocalStorage _storage;

    public LocalStorageService()
    {
        var config = new LocalStorageConfiguration()
        {
            AutoLoad = true,
            AutoSave = true,
            Filename = "SHORTLINKBA"
        };

        _storage = new LocalStorage(config);
    } 
    #endregion

    public void ClearStorage(List<string> keys)
    {
        foreach (var key in keys)
        {
            _storage.Remove(key);
        }
    }

    public bool Exists(string key)
    {
        return _storage.Exists(key);
    }

    public T GetStorageValue<T>(string key)
    {
        try
        {
            return _storage.Get<T>(key);
        }
        catch
        {
            return default(T);
        }
    }

    public void SetStorageValue<T>(string key, T value)
    {
        _storage.Store(key, value);
        _storage.Persist();
    }

    public void SetStorageValueWithExpiry<T>(string key, T value, TimeSpan expiry)
    {
        var expiryDate = DateTime.UtcNow.Add(expiry);
        var data = new StorageDataDto<T>
        {
            Value = value,
            Expiry = expiryDate
        };
        var jsonData = JsonSerializer.Serialize(data);
        SetStorageValue(key, jsonData);
    }

    public T GetStorageValueWithExpiry<T>(string key)
    {
        var jsonData = GetStorageValue<string>(key);
        if (jsonData == null)
        {
            return default(T);
        }

        var data = JsonSerializer.Deserialize<StorageDataDto<T>>(jsonData);
        if (data.Expiry < DateTime.UtcNow)
        {
            // Token has expired
            _storage.Remove(key);
            _storage.Persist();
            return default(T);
        }

        return data.Value;
    }
}