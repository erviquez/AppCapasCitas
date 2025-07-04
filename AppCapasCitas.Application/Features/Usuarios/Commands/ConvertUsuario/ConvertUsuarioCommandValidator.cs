using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ConvertUsuario;

public class ConvertUsuarioCommandValidator : AbstractValidator<ConvertUsuarioCommand>
{
    public ConvertUsuarioCommandValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El campo UsuarioId es obligatorio.")
            .NotNull().WithMessage("El campo UsuarioId no puede ser nulo.");
        RuleFor(x => x.UsuarioAccion)
            .NotEmpty().WithMessage("El campo CreadoPor es obligatorio.")
            .NotNull().WithMessage("El campo CreadoPor no puede ser nulo.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("El campo RoleId es obligatorio.")
            .NotNull().WithMessage("El campo RoleId no puede ser nulo.");

    }

}
