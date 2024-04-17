using Core.Interfaces.Services;
using Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserRequestController : BaseApiController
{
    private readonly IUserRequestService _service;

    public UserRequestController(IUserRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequestModel request)
    {
        return Ok(await _service.Add(request));
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var userRequests = await _service.GetAll();
        return Ok(userRequests);
    }
}
