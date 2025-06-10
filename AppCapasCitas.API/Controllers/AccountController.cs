using System.Net;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;
using AppCapasCitas.Application.Features.Usuarios.Commands.DisableUsuario;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Request.Usuario;
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
        var result = await _authService.Login(request);
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
        {
            result!.IsSuccess = false;
            result.Message = "Error interno al registrar el usuario.";
            return result;

        }
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
        var result = await _authService.Logout(request);
        return Accepted(result);

    }

    [HttpPut("DisableUsuarioById", Name = "DisableUsuarioById")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DisableUsuarioById([FromBody] UsuarioRequest request)
    {
        var response = new Response<bool>();
        if (request == null || request.UserId == Guid.Empty)
        {
            response.IsSuccess = false;
            response.Message = "Invalid user request.";
            return BadRequest(response);
        }
        var query = new DisableUsuarioCommand { IdentityId = request.UserId!, Active = request.IsActive };
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {            
            return NotFound(result);
        }
        return Ok(result);
    }

}
