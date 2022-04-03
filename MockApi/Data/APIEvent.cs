namespace MockApi.Data;

public class APIEvent
{
    public string Endpoint { get; set; }
    public string Query { get; set; }
    public string Method { get; set; }
    public DateTime QueryTime { get; set; }
}