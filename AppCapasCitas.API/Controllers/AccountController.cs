using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
 public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    //[ValidateAntiForgeryToken]

    public async Task<ActionResult<Response<AuthResponse>>> Login([FromBody] AuthRequest request)
    { 
        var result =  await _authService.Login(request);
        if (result == null)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al acceder las credenciales.");
        if (!result.IsSuccess)
            return NotFound(result);
        return Accepted(result);
    }
    [HttpPost("Register")]
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]    
    public async Task<ActionResult<Response<RegistrationResponse>>> Register([FromBody] CreateUsuarioCommand command)
    {
        var result = await _mediator.Send(command);

        if (result == null)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al registrar el usuario.");

        if (!result.IsSuccess)
            return BadRequest(result); 
        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] TokenRequest request)
    {
        return Ok(await _authService.RefreshToken(request));
    }
    [HttpPost("Logout")]
    public async Task<ActionResult<bool>> Logout([FromBody] LogoutRequest request)
    {
        return Accepted(await _authService.Logout(request));
    }

}
