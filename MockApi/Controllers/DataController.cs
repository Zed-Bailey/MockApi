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
    private string RowsToJson(IEnumerable<DynamicRow_rewrite> rows)
    {
        // defintley a better way to do this, but this is working for now
        var json = "[\n";
        foreach (var r in rows) json += r.ToJson() + ",";
        
        // remove any trailing commas for the formatter
        json = json.TrimEnd(',');
        json += "]";
 
        // pretty formatting the json
        // https://stackoverflow.com/a/67928315
        using var jsonDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions{WriteIndented = true});
 
    }
        

    [HttpGet("select")]
    public IActionResult GetQuery([FromQuery(Name = "limit")] string limit, [FromQuery(Name = "where")] string columnName, [FromQuery(Name = "is")] object equalTo)
    {
        var invalidNumber = BadRequest(new {error = "invalid limit amount passed in. valid options are 'all' to return all rows or an int value > 0"});
        
        var amount = -1;
        if (limit == "all")
            amount = _service.Rows.Count;
        else
        {
            // try and parse the string into an int, if it fails return a bad request
            if (!int.TryParse(limit, out amount))
            {
                return invalidNumber;
            }
            // make sure we have a positive number to return
            if (amount < 0) return invalidNumber;
            
            // check that the amount value is not larger then the number of rows we have
            if (amount > _service.Rows.Count) amount = _service.Rows.Count;
            
        }
        
        // amount is now a valid value, check to see if the column exists
        if (!_service.ColumnExists(columnName))  return BadRequest(new {error = $"The column with name {columnName} does not exist!"});

        // todo: this find all method is not correctly finding the rows :(
        var matched = _service.FindAll(columnName, equalTo as string);
        Console.WriteLine(matched.Count());
        return Ok(RowsToJson(matched));
    }

    [HttpGet]
    public IActionResult GetAllRows()
    {
        if (_service.Rows.Count == 0) return Ok("[]");
        
        
        
        
        return Ok(RowsToJson(_service.Rows));
    }
}