namespace MiniProfiler_Test.Models.Storage;

public class StorageDataDto<T>
{
    public T Value { get; set; }
    public DateTime Expiry { get; set; }
}