using Microsoft.AspNetCore.Mvc;
using MockApi.Data;
using MockApi.Services;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;

namespace MockApi.Controllers;
public class PostRowBinding
{
    public Dictionary<string, string> Data;
}



[ApiController]
[Route("/api/[controller]")]
public class DataController : ControllerBase
{
    

    private readonly ILogger<DataController> _logger;
    private readonly DataService _service;
    
    public DataController(ILogger<DataController> logger, DataService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Formats the json properly
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public string FormatJson(string json)
    {
        // pretty formatting the json
        // https://stackoverflow.com/a/67928315
        using var jsonDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions{WriteIndented = true}); 
    }
    
    /// <summary>
    /// Serialize the rows to json
    /// </summary>
    /// <param name="rows">rows to serialize</param>
    /// <returns>a json string with the rows serialized in it</returns>
    private string RowsToJson(IEnumerable<DynamicRow> rows)
    {
        // defintley a better way to do this, but this is working for now
        var json = "[\n";
        foreach (var r in rows) json += r.ToJson() + ",";
        
        // remove any trailing commas for the formatter
        json = json.TrimEnd(',');
        json += "]";
        
        return FormatJson(json);
    }

    /// <summary>
    /// Creates a json response that can be returned by an endpoint
    /// sets the status code
    /// </summary>
    /// <param name="json">json string to add to response</param>
    /// <param name="statusCode">the responses status, defaults to 200 == OK</param>
    /// <returns>an IActionResult that can be returned from an api endpoint</returns>
    public IActionResult JsonResponse(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        
        var response =  Content(json, "application/json");

        response.StatusCode = (int) statusCode;
        return response;
    }

    /// <summary>
    /// Creates a json response with a single key and value
    /// </summary>
    /// <param name="key">json key</param>
    /// <param name="value">value to go with key</param>
    /// <param name="statusCode">Status code of response</param>
    /// <returns>IActionResult that can be returned by an api endpoint</returns>
    public IActionResult JsonResponse(string key, string value, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = $"{{ \"{key}\" :\"{value}\"}}";
        return JsonResponse(json, statusCode);
    }
    

    // GET /api/data/:id
    [HttpGet("{id:int}")]
    public IActionResult GetRowWithID(int id)
    {
        var noMatchingIDResponse = BadRequest(new {error = $"No row with ID == {id} could be found"});

        // find a row with matching ID, if null return a 404 response with an appropriate error message
        var row = _service.Rows.FirstOrDefault(x => x.RowID == id);
        if (row is null) return noMatchingIDResponse;
        var json = row.ToJson();
        
        return JsonResponse(json);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRow(int id)
    {
        var row = _service.Rows.FirstOrDefault(x => x.RowID == id);
        if (row is null)
            return JsonResponse($"{{\"error\": \"No row with id == {id} could be found\"}}", HttpStatusCode.BadRequest);

        return _service.Rows.Remove(row) ? Ok() : JsonResponse("error", $"Failed to remove the row with id: {id}", HttpStatusCode.InternalServerError);
    }

  
    [HttpPut("{id:int}")]
    public IActionResult UpdateRow(int id, Dictionary<string,string> data )
    {
        var row = _service.Rows.FirstOrDefault(x => x.RowID == id);
        if (row is null)
            return JsonResponse($"{{\"error\": \"No row with id == {id} could be found\"}}", HttpStatusCode.BadRequest);

        foreach (var dataKey in data.Keys)
        {
            row.UpdateColumnApi(dataKey, data[dataKey]);
        }
        
        return Ok();
    }
    

    // GET /api/data/select?where={columnName}&is={value}
    // optional parameter limit
    [HttpGet("select")]
    public IActionResult GetQuery([FromQuery(Name = "where")] string columnName, [FromQuery(Name = "is")] string equalTo, int? limit = null)
    {
        var invalidNumber = BadRequest(new {error = "invalid limit amount passed in. Has to be an int value > 0"});
        
        // check if a value was passed in
        if (limit is null)
            limit = _service.Rows.Count;
        
        // make sure we have a positive number to return
        if (limit < 0)
            return invalidNumber;
        
        // check that the amount value is not larger then the number of rows we have
        if (limit > _service.Rows.Count)
            limit = _service.Rows.Count;

        // amount is now a valid value, check to see if the column exists
        if (!_service.ColumnExists(columnName))  return BadRequest(new {error = $"The column with name {columnName} does not exist!"});
        
        var matched = _service.FindAll(columnName, equalTo).Take(limit.Value);
        
        return JsonResponse(RowsToJson(matched));

    }
    

    // GET /api/data/
    [HttpGet]
    public IActionResult GetAllRows()
    {
        if (_service.Rows.Count == 0)
            return JsonResponse("[]");

        
        return JsonResponse(RowsToJson(_service.Rows));
    }
}