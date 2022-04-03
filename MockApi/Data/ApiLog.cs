namespace MockApi.Data;

public class ApiLog
{
    public string Endpoint { get; set; }
    public string Query { get; set; }
    public string Method { get; set; }
    public DateTime QueryTime { get; set; }
}