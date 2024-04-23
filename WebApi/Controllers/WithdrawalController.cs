using Core.Interfaces.Services;
using Core.Request;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class WithdrawalController : BaseApiController
{
    private readonly IWithdrawalService _service;
    public WithdrawalController(IWithdrawalService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawalModel request)
    {
        return Ok(await _service.Add(request));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var withdrawals = await _service.GetAll();
        return Ok(withdrawals);
    }
}