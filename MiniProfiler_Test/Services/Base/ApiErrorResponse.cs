namespace MiniProfiler_Test.Services.Base;

public class ApiErrorResponse
{
    public string Message { get; set; }
    public List<ValidationError> Errors { get; set; } = new();
}