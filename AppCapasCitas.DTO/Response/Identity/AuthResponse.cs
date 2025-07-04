

namespace AppCapasCitas.DTO.Response.Identity;

public class AuthResponse
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    //public string RoleId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Active { get; set; } 
    public DateTime? LastLogin { get; set; } = null;
    public List<string> Roles { get; set; } = new List<string>();
}

