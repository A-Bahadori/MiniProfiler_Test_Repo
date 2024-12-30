namespace MiniProfiler_Test.Services.Base;

public class Response<T>
{
    public string? Message { get; set; }
    public object? Errors { get; set; }
    public List<ValidationError> ValidationErrors { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }
    public int? StatusCode { get; set; }
}