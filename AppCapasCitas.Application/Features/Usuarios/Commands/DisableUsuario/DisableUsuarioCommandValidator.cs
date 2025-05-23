using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.DisableUsuario.DisableUsuario;

public class DisableUsuarioCommandValidator : AbstractValidator<DisableUsuarioCommand>
{
    public DisableUsuarioCommandValidator()
    {
        RuleFor(p => p.IdentityId)
            .NotEmpty().WithMessage("El ID de usuario es requerido");
    }
}
