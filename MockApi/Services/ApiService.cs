using MockApi.Data;

namespace MockApi.Services;

public class ApiService
{
    public bool API_ENABLED;
    public string Host;
    
    public List<ApiLog> Events { get; } = new();
    
    public void ClearLogs() {
        Events.Clear();
    }
    

    /// <summary>
    /// Fetches data from request and create an ApiLog object
    /// </summary>
    /// <param name="request">the created request</param>
    public void AddLog(HttpRequest request)
    {
        var log = new ApiLog
        {
            Endpoint = request.Path.ToString(),
            Method = request.Method,
            QueryTime = DateTime.Now,
            Query = request.QueryString.ToString()
        };
        
        Events.Add(log);
    }
    
}