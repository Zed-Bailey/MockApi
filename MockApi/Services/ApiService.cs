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
    
    public void AddLog() {
    }
    
}