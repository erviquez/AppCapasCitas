using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.DTO.Request.Identity;

public class RegistrationRequest
{
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [MaxLength(50, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(3, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    public string Username { get; set; } = string.Empty;



    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [DataType(DataType.Password)]
    [MaxLength(100, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(6, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Debe confirmar el password")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;




    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    public string Email { get; set; } = string.Empty;

    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;


    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [Display(Name = "Nombre Completo")]
    public string Nombre { get; set; } = string.Empty;
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    public string Apellido { get; set; } = string.Empty;
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [Phone(ErrorMessage = "El número de teléfono no es válido.")]

    [MaxLength(15, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(10, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    [DisplayName("Teléfono")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\+?[0-9\s\-()]+$", ErrorMessage = "El número de teléfono solo puede contener dígitos, espacios, guiones y paréntesis.")]
    public string Telefono { get; set; } = string.Empty;
    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [Phone(ErrorMessage = "El número de celular no es válido.")]
    [MaxLength(15, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(10, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    [DisplayName("Celular")]
    [DataType(DataType.PhoneNumber)]
    public string Celular { get; set; } = string.Empty;

    [Required(ErrorMessage = ConstantError.ErrorMessageInput)]
    [MaxLength(200, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(5, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    [DisplayName("Dirección")]
    [DataType(DataType.MultilineText)]
    public string Direccion { get; set; } = string.Empty;
    [MaxLength(100, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(2, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    [DisplayName("Dirección")]
    public string Ciudad { get; set; } = string.Empty;
    [MaxLength(100, ErrorMessage = ConstantError.ErrorMessageMaxLengthInput)]
    [MinLength(2, ErrorMessage = ConstantError.ErrorMessageMinLengthInput)]
    [DisplayName("Estado/Provincia")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = ConstantError.ErrorMessageCombo)]
    [DisplayName("País")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un país válido")]
    public int CodigoPais { get; set; } = 0;
    public Guid UsuarioCreacionId { get; set; }
}
    

