using Core.Interfaces.Services;
using Core.Request;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class BusinessController : BaseApiController
{
    private readonly IBusinessService _businessService;

    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessModel request)
    {
        return Ok(await _businessService.Add(request));
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBusinessModel request)
    {
        return Ok(await _businessService.Update(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _businessService.Delete(id));
    }
}
