using Core.Interfaces.Services;
using Core.Request;
using Core.Requests;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PromotionController : BaseApiController
{
    private readonly IPromotionService _promotionService;

    public PromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }


    [HttpGet("filtered")]
    public async Task<IActionResult> GetFiltered([FromQuery] FilterPromotionModel filter)
    {
        var promotions = await _promotionService.GetFiltered(filter);
        return Ok(promotions);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePromotionModel request)
    {
        return Ok(await _promotionService.Add(request));
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePromotionModel request)
    {
        return Ok(await _promotionService.Update(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _promotionService.Delete(id));
    }
}
