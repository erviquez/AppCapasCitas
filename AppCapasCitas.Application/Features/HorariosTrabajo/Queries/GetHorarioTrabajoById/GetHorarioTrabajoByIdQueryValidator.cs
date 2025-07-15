using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.HorariosTrabajo.Queries.GetHorarioTrabajoById;

public class GetHorarioTrabajoByIdQueryValidator : AbstractValidator<GetHorarioTrabajoByIdQuery>
{
    public GetHorarioTrabajoByIdQueryValidator()
    {
        RuleFor(x => x.HorarioTrabajoId)
            .NotEmpty().WithMessage("El Id del horario de trabajo no puede estar vacío")
            .NotEqual(Guid.Empty).WithMessage("El Id del horario de trabajo no puede ser un GUID vacío");
    }

}
