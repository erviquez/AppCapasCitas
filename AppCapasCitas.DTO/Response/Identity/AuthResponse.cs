

namespace AppCapasCitas.DTO.Response.Identity;

public class AuthResponse
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Active { get; set; } 
    

}

