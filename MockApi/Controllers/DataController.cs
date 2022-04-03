using Microsoft.AspNetCore.Mvc;
using MockApi.Data;
using MockApi.Services;

namespace MockApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DataController : ControllerBase
{
    
    private readonly ILogger<DataController> _logger;
    private readonly DataService _service;
    
    public DataController(ILogger<DataController> logger, DataService service)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "Hello world";
    }
}