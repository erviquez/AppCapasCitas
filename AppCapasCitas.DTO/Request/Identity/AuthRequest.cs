namespace AppCapasCitas.DTO.Request.Identity;

public class AuthRequest
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
    public bool Active { get; set; } = true;
    
}
