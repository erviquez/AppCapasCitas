using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;

public class ActionLogUsuarioCommandValidator : AbstractValidator<ActionLogUsuarioCommand>
{
    public ActionLogUsuarioCommandValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El Id del usuario es requerido.")
           .NotEqual(Guid.Empty).WithMessage("El Id del usuario no puede ser un GUID vacío.");

        RuleFor(x => x.TipoAccion)
            .NotEmpty().WithMessage("El tipo de acción es requerido.");
    }

}
