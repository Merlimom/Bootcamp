using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : BaseApiController
{
    private readonly IJwtProvider _jwtProvider;

    public AuthController(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    //[HttpGet("generate-token")]
    //[AllowAnonymous]
    //public IActionResult Generate()
    //{
    //    string token = _jwtProvider.Generate();

    //    return Ok(token);
    //}

    [HttpGet("generate-token")]
    [AllowAnonymous]
    public IActionResult Generate([FromQuery] IEnumerable<string> roles)
    {
        if (roles == null || !roles.Any())
        {
            return BadRequest("Se debe asignar al menos un rol");
        }
        string token = _jwtProvider.Generate(roles);

        return Ok(token);
    }

    [HttpGet("protected-endpoint")]
    [Authorize]
    public IActionResult ProtectedEndpoint()
    {

        return Ok("Este es un endpoint protegido");
    }

    [HttpGet("protected-endpoint-seguridad")]
    [Authorize(Roles = "Seguridad")]
    public IActionResult ProtectedEndpoint2()
    {

        return Ok("Este solo puede acceder un miembro de Seguridad");
    }

    [HttpGet("protected-endpoint-invitado")]
    [Authorize(Roles = "Invitado")]
    public IActionResult ProtectedEndpoint5()
    {

        return Ok("Este solo puede acceder un invitado");
    }

    [HttpGet("protected-endpoint-admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult ProtectedEndpoint3()
    {

        return Ok("Este solo puede acceder un Administrador");
    }


    [HttpGet("protected-endpoint-todos")]
    [Authorize(Roles = "Admin, Seguridad, Invitado")]
    public IActionResult ProtectedEndpoint4()
    {

        return Ok("Este endpoint pueden ver Admin, Seguridad e Invitado");
    }
}