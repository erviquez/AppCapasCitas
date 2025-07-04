
namespace AppCapasCitas.DTO.Request.Identity;
public class LogoutRequest
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string Token { get; set; } = string.Empty ;
}
