using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.DTO.Request.Identity;

public class AuthRequest
{
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "La contraseña debe tener al menos 6 caracteres, una mayúscula, una minúscula, un número y un carácter especial.")]
    [MaxLength(50, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(6, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
    public bool Active { get; set; } = true;
    
}
