using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioByIdentityId;

public class GetUsuarioByIdentityIdQueryValidator : AbstractValidator<GetUsuarioByIdentityIdQuery>
{
    public GetUsuarioByIdentityIdQueryValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El Id del usuario no puede estar vacío.")
            .NotNull().WithMessage("El Id del usuario no puede ser nulo.")
            .Must(x => x != Guid.Empty).WithMessage("El Id del usuario no puede ser un GUID vacío.");
    }

}
