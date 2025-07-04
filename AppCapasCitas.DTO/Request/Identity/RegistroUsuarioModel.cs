// using System;
// using System.ComponentModel.DataAnnotations;

// namespace AppCapasCitas.DTO.Request.Identity;

// public class RegistroUsuarioModel
// {
//     [Required(ErrorMessage = "El password es obligatorio")]
//     public string Password { get; set; } = string.Empty;

//     [Required(ErrorMessage = "Debe confirmar el password")]
//     [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
//     public string ConfirmPassword { get; set; } = string.Empty;

//     // ...otros campos del RegistrationRequest
//     // Puedes heredar de RegistrationRequest o mapear manualmente
// }