using Microsoft.AspNetCore.Mvc;
using MockApi.Data;
using MockApi.Services;
using System.Linq;
using System.Text.Json;

namespace MockApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DataController : ControllerBase
{
    
    /*
     * TODO: figure out how to have multiple get requests
     * Example queries
     * api/data?select=all&where=column&is=test
     * select * rows from data where column == test
     *
     * api/data?select=1&where=column&is=test
     * select * rows from data where column == test limit 1
     */
    private readonly ILogger<DataController> _logger;
    private readonly DataService _service;
    
    public DataController(ILogger<DataController> logger, DataService service)
    {
        _logger = logger;
        _service = service;
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

    public string FormatJson(string json)
    {
        // pretty formatting the json
        // https://stackoverflow.com/a/67928315
        using var jsonDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions{WriteIndented = true}); 
    }

    // GET /api/data/:id
    [HttpGet("{id}")]
    public IActionResult GetRowWithID(string id)
    {
        var noMatchingIDResponse = BadRequest(new {error = $"No row with ID == {id} could be found"});

        var row = _service.Rows.FirstOrDefault(x => x.RowID.ToString() == id);
        if (row is null) return noMatchingIDResponse;
        var json = row.ToJson();
        return Ok(FormatJson(json));
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
        
        Console.WriteLine($"DEBUG :: Finding all rows where {columnName} == {equalTo}");
        
        
        var matched = _service.FindAll(columnName, equalTo).Take(limit.Value);
        
        return Ok(RowsToJson(matched));
    }

    // GET /api/data/
    [HttpGet]
    public IActionResult GetAllRows()
    {
        if (_service.Rows.Count == 0) 
            return Ok("[]");
        
        return Ok(RowsToJson(_service.Rows));
    }
}