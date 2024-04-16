using Core.Interfaces.Services;
using Core.Request;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class EnterpriseController : BaseApiController
{
    private readonly IEnterpriseService _enterpriseService;

    public EnterpriseController(IEnterpriseService enterpriseService)
    {
        _enterpriseService = enterpriseService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var enterprise = await _enterpriseService.GetById(id);
        return Ok(enterprise);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEnterpriseModel request)
    {
        return Ok(await _enterpriseService.Add(request));
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEnterpriseModel request)
    {
        return Ok(await _enterpriseService.Update(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _enterpriseService.Delete(id));
    }
}
