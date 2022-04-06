using System.Collections.ObjectModel;
using MockApi.Data;

namespace MockApi.Services;

public class ApiService
{
    public bool ApiEnabled;
    public string Host;
    
    public readonly ObservableCollection<ApiLog> Events = new();
    
    /// <summary>
    /// Clear the logs
    /// </summary>
    public void ClearLogs() {
        Events.Clear();
    }


    public void AddLog(ApiLog log) => Events.Insert(0, log);
   
}