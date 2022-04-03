using MockApi.Data;

namespace MockApi.Services;

public class ApiService
{
    public bool API_ENABLED;
    public string Host;
    
    public List<APIEvent> Events { get; } = new();
    
    public void ClearLogs() {
        Events.Clear();
    }
    
    // TODO: implement extracting data from request and creating a log
    public void AddLog() {
    }
    
}