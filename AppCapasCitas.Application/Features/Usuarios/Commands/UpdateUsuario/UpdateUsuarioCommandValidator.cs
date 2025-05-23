
using System.Text.RegularExpressions;
using AppCapasCitas.Application.Helpers;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{
    
     public UpdateUsuarioCommandValidator()
    {
        // Validación para Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido")
            .MinimumLength(5).WithMessage("El nombre de usuario debe tener al menos 5 caracteres")
            .MaximumLength(20).WithMessage("El nombre de usuario no puede exceder los 20 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Solo se permiten letras, números y guiones bajos");

        // Validación para Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
            .MaximumLength(50).WithMessage("La contraseña no puede exceder los 50 caracteres")
            .Matches("[A-Z]").WithMessage("Debe contener al menos una letra mayúscula")
            .Matches("[a-z]").WithMessage("Debe contener al menos una letra minúscula")
            .Matches("[0-9]").WithMessage("Debe contener al menos un número")
            .Matches("[^a-zA-Z0-9]").WithMessage("Debe contener al menos un carácter especial")
            .NotEqual(x => x.Username).WithMessage("La contraseña no puede ser igual al nombre de usuario");


        // Validación para Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("Formato de email inválido")
            .MaximumLength(100).WithMessage("El email no puede exceder los 100 caracteres");

        // Validación para Nombre
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        // Validación para Apellido
        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
            .WithMessage("El apellido solo puede contener letras y espacios");

        // Validación para Teléfono fijo (opcional)
        RuleFor(x => x.Telefono)
            .Must(BeAValidPhoneNumber).When(x => !string.IsNullOrEmpty(x.Telefono))
            .WithMessage("Formato de teléfono inválido")
            .MaximumLength(15).WithMessage("El teléfono no puede exceder los 15 caracteres");

        // Validación para Celular
        RuleFor(x => x.Celular)
            .NotEmpty().WithMessage("El celular es requerido")
            .Must(BeAValidPhoneNumber).WithMessage("Formato de celular inválido")
            .MaximumLength(15).WithMessage("El celular no puede exceder los 15 caracteres");

        // Validación para Dirección (opcional)
        RuleFor(x => x.Direccion)
            .MaximumLength(100).WithMessage("La dirección no puede exceder los 100 caracteres");

        // Validación para Ciudad (opcional)
        RuleFor(x => x.Ciudad)
            .MaximumLength(50).WithMessage("La ciudad no puede exceder los 50 caracteres");

        // Validación para Estado/Provincia (opcional)
        RuleFor(x => x.Estado)
            .MaximumLength(50).WithMessage("El estado no puede exceder los 50 caracteres");

        // Validación para Código de País
        RuleFor(x => x.CodigoPais)
            .NotEmpty().WithMessage("El código de país es requerido")
            .InclusiveBetween(1, 999).WithMessage("Código de país inválido")
            .Must(BeAValidCountryCode).WithMessage("Código de país no reconocido");
    }
      private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeAValidPhoneNumber(string phoneNumber)
    {
        // Expresión regular simplificada para números internacionales
        return Regex.IsMatch(phoneNumber, @"^\+?[0-9\s\-\(\)]{7,}$");
    }

    private bool BeAValidCountryCode(int codigoPais)
    {
        // Verifica contra tu helper de países
        return PaisesHelper.GetNombrePais(codigoPais) != "Desconocido";
    }

}
