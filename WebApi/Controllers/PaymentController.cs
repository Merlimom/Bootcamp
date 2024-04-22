using Core.Interfaces.Services;
using Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PaymentController : BaseApiController
{
    private readonly IPaymentService _service;
    public PaymentController(IPaymentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentModel request)
    {
        return Ok(await _service.Add(request));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _service.GetAll();
        return Ok(payments);
    }
}
