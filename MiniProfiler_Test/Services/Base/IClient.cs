namespace MiniProfiler_Test.Services.Base;

public partial interface IClient
{
    public HttpClient HttpClient { get; }
}