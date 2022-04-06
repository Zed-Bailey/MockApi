using MockApi.Services;

namespace MockApi.Data;

// Microsoft documentation on custom middleware classes: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-6.0
public class LoggerMiddleWare
{
    private readonly RequestDelegate _next;
    private ApiService _service;
    
    public LoggerMiddleWare(RequestDelegate next, ApiService service)
    {
        _next = next;
        // ApiService is injected through dependency injection!
        _service = service;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        // ignore any request that isn't for the api route
        if (!context.Request.Path.ToString().Contains("/api/data"))
        {
            await _next(context);
            return;
        }
        
        // Streaming the response body: https://stackoverflow.com/a/43404745
        Stream originalBody = context.Response.Body;
        string responseBody;
        try
        {
            await using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            memStream.Position = 0;
            responseBody = new StreamReader(memStream).ReadToEnd();
            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);
        } finally {
            context.Response.Body = originalBody;
        }

        var request = context.Request;
        var response = context.Response;
        // create log and add it to the service
        var log = new ApiLog
        {
            Endpoint = request.Path,
            Query = request.QueryString.ToString(),
            Method = request.Method,
            QueryTime = DateTime.Now,
            ResponseCode = response.StatusCode,
            ResponseJson = responseBody
        };
        
        _service.AddLog(log);
    }
}

public static class LoggerMiddleWareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleWare>();
    }
}
