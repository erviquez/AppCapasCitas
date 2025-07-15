using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadById;

public class GetEspecialidadByIdQueryValidator : AbstractValidator<GetEspecialidadByIdQuery>
{
    public GetEspecialidadByIdQueryValidator()
    {
        RuleFor(x => x.EspecialidadId)
            .NotEmpty().WithMessage("El ID de la especialidad no puede estar vacío.")
            .NotNull().WithMessage("El ID de la especialidad no puede ser nulo.")
            .Must(BeAValidGuid).WithMessage("El ID de la especialidad debe ser un GUID válido.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }

}
