using Core.Interfaces.Services;
using Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class MovementController : BaseApiController
{
    private readonly IMovementService _service;

    public MovementController(IMovementService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovementModel request)
    {
        return Ok(await _service.Add(request));
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var movements = await _service.GetAll();
        return Ok(movements);
    }
}
