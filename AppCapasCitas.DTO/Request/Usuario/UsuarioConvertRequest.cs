
using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.DTO.Request.Usuario;

public class UsuarioConvertRequest
{
    // [Required(ErrorMessage = "El UsuarioId es obligatorio.")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "El RoleId es obligatorio.")]
    public Guid RoleId { get; set; }
    public string? Username { get; set; }
    public string? RoleName { get; set; }


    // [Required(ErrorMessage = "El usuario que realiza la acción es obligatorio.")]
    // [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder los 100 caracteres.")]
    public string? UsuarioAccion { get; set; }
}