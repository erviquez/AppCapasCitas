
namespace AppCapasCitas.DTO.Request.Identity;

public class AuthResetPasswordRequest
{
    public Guid UsuarioId { get; set; } = Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;



}
