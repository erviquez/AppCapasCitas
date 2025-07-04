using System.Net;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;
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
        if (result.Data!.Id != Guid.Empty)
            await LogAccion(result.Data.Id, result.Data.Id.ToString());

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
        if (result.Data!.Id != Guid.Empty)
        {
            await LogAccion(result.Data.Id, command.UsuarioCreacionId.ToString(), "registro de usuario");
        }
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
        if (request.UserId != Guid.Empty)
            await LogAccion(request.UserId, request.UserId.ToString());
        return Accepted(result);

    }

    [HttpPut("DisableUsuarioById", Name = "DisableUsuarioById")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DisableUsuarioById([FromBody] UsuarioRequest request)
    {
        var response = new Response<bool>();
        if (request == null || request.UsuarioId == Guid.Empty)
        {
            response.IsSuccess = false;
            response.Message = "Invalid user request.";
            return BadRequest(response);
        }
        var query = new DisableUsuarioCommand { IdentityId = request.UsuarioId!, Active = request.Activo };
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {
            return NotFound(result);
        }
        string active = request.Activo ? "EnableUsuarioById" : "DisableUsuarioById";

        if (request.UsuarioId != Guid.Empty)
            await LogAccion(request.UsuarioId, request.UsuarioAccion, active);
        return Ok(result);
    }

    // <summary>
    //confirmation email
    // </summary>
    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string usuarioId, string token)
    {
        if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid user ID or token.");
        }
        var result = await _authService.ConfirmEmail(usuarioId, token);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        //verificar si se puede convertir a guid
        if (!Guid.TryParse(usuarioId, out var guidUsuarioId) || guidUsuarioId == Guid.Empty)
        {
            return BadRequest("Invalid user ID format.");
        }
        if (guidUsuarioId != Guid.Empty)
            await LogAccion(guidUsuarioId, result.Data.ToString());
            
        return Ok("Email confirmed successfully.");
    }
    /// <summary>
    /// Confirmación de teléfono vía SMS
    /// </summary>
    [HttpGet("ConfirmPhone")]
    public async Task<IActionResult> ConfirmPhone(string usuarioId, string token)
    {
        if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("ID de usuario o token inválido.");
        }
        var result = await _authService.ConfirmPhoneNumber(usuarioId, token);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        //verificar si se puede convertir a guid
        if (!Guid.TryParse(usuarioId, out var guidUsuarioId) || guidUsuarioId == Guid.Empty)
        {
            return BadRequest("Invalid user ID format.");
        }
        if (guidUsuarioId != Guid.Empty)
            await LogAccion(guidUsuarioId, "DisableUsuarioById", result.Data.ToString());
        return Ok("Teléfono confirmado correctamente.");
    }


    private async Task LogAccion(Guid usuarioId, string? usuarioCreacion = "system", [System.Runtime.CompilerServices.CallerMemberName] string? methodName = null, string? mensaje = null)
    {
        var logActionCommand = new ActionLogUsuarioCommand
        {
            UsuarioId = usuarioId,
            TipoAccion = methodName ?? "Desconocido",
            UsuarioCreacion = usuarioCreacion
        };
        await _mediator.Send(logActionCommand);
    }

}
