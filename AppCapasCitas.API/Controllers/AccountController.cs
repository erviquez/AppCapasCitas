using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
 public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator, IAuthService authService, IAsyncRepository<Usuario> userRepository)
    {
        _mediator = mediator;
        _authService = authService;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    //[ValidateAntiForgeryToken]

    public async Task<ActionResult<Response<AuthResponse>>> Login([FromBody] AuthRequest request)
    { 
        return Ok( await _authService.Login(request));
    }
    [HttpPost("Register")]
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]    
    public async Task<ActionResult<Response<RegistrationResponse>>> Register([FromBody] CreateUsuarioCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok( result);

    }

    [HttpPost("RefreshToken")]
    // [ValidateAntiForgeryToken]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] TokenRequest request)
    {
        return Ok(await _authService.RefreshToken(request));
    }
    [HttpPost("Logout")]
    // [ValidateAntiForgeryToken]
    public async Task<ActionResult<bool>> Logout([FromBody] LogoutRequest request)
    {
        return Ok(await _authService.Logout(request));
    }

}
