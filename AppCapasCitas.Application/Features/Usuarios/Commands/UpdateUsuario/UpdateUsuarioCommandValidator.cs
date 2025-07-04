
using System.Text.RegularExpressions;
using AppCapasCitas.DTO.Helpers;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{

    public UpdateUsuarioCommandValidator()
    {

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
        // Validación para País (opcional, se infiere del código de país)
        RuleFor(x => x.Pais)
            .Must((x, pais) => PaisesHelper.GetNombrePais(x.CodigoPais) == pais)
            .WithMessage("País no coincide con el código de país");
        
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
