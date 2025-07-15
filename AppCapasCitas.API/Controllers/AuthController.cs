using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Endpoint de prueba para verificar JWT
    /// </summary>
    [HttpGet("test")]
    [Authorize]
    public IActionResult TestAuth()
    {
        var userId = User.FindFirst("uid")?.Value;
        var userName = User.FindFirst("name")?.Value;
        var userEmail = User.FindFirst("email")?.Value;
        
        return Ok(new
        {
            Message = "Token JWT válido",
            UserId = userId,
            UserName = userName,
            UserEmail = userEmail,
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
        });
    }

    /// <summary>
    /// Iniciar sesión y obtener token JWT
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        try
        {
            var response = await _authService.Login(request);
            
            if (!response.IsSuccess)
                return BadRequest(response);
                
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error interno: {ex.Message}" });
        }
    }
}