using Core.Interfaces.Services;
using Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class DepositController : BaseApiController
{
    private readonly IDepositService _service;

    public DepositController(IDepositService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepositModel request)
    {
        return Ok(await _service.Add(request));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var deposits = await _service.GetAll();
        return Ok(deposits);
    }
}
