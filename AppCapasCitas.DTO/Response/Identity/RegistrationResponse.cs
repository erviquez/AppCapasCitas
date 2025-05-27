using FluentValidation.Results;

namespace AppCapasCitas.DTO.Response.Identity;

public class RegistrationResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;   
    public bool Success { get; set; } = false;
    public IEnumerable<ValidationFailure>? Errors { get; set; }

}
