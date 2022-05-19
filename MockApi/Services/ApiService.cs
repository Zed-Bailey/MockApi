using System.Collections.ObjectModel;
using MockApi.Data;

namespace MockApi.Services;

public class ApiService
{
    public bool ApiEnabled;
    public string Host = "";
    
    public readonly ObservableCollection<ApiLog> Events = new();
    
    /// <summary>
    /// Clear the logs
    /// </summary>
    public void ClearLogs() {
        Events.Clear();
    }


    /// <summary>
    /// add a new log to the services log collection
    /// </summary>
    /// <param name="log">the enw log to insert</param>
    public void AddLog(ApiLog log) => Events.Insert(0, log);
   
}