namespace MockApi.Data;

public class ApiLog
{
    public string Endpoint { get; init; }
    public string Query { get; init; }
    public string Method { get; init; }
    public DateTime QueryTime { get; init; }
}