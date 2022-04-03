using Microsoft.AspNetCore.Mvc;
using MockApi.Data;
using MockApi.Services;

namespace MockApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DataController : ControllerBase
{
    
    /*
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
    }

    [HttpGet]
    public IActionResult Get([FromQuery(Name = "select")] string selectAmount, [FromQuery(Name = "where")] string columnName, [FromQuery(Name = "is")] string equalTo)
    {
        var invalidNumber = BadRequest(new {error = "invalid select amount passed in,  valid options are 'all' or int value > 0"});
        
        var amount = -1;
        if (selectAmount == "all")
            amount = _service.Rows.Count;
        else
        {
            // try and parse the string into an int, if it fails return a bad request
            if (!int.TryParse(selectAmount, out amount))
            {
                return invalidNumber;
            }
            
            if (amount < 0) return invalidNumber;
            
            // check that the amount value is not larger then the number of rows we have
            if (amount > _service.Rows.Count) amount = _service.Rows.Count;
            
        }
        
        return Ok();
    }
}