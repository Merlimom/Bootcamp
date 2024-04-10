using Core.Interfaces.Services;
using Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers; 

public class CreditCardController : BaseApiController
{
    private readonly ICreditCardService _service;

    public CreditCardController(ICreditCardService service)
    {
        _service = service;    
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCreditCardModel request)
    {
        return Ok(await _service.Add(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var creditCard = await _service.GetById(id);
        return Ok(creditCard);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFiltered([FromQuery] FilterCreditCardModel filter)
    {
        var creditCards = await _service.GetFiltered(filter);
        return Ok(creditCards);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCreditCardModel request)
    {
        return Ok(await _service.Update(request));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _service.Delete(id));
    }
}
