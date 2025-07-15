using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidadCosto;

public class UpdateEspecialidadCostoCommandValidator : AbstractValidator<UpdateEspecialidadCostoCommand>
{
    public UpdateEspecialidadCostoCommandValidator()
    {
        RuleFor(x => x.EspecialidadId)
            .NotEmpty().WithMessage("El ID de la especialidad es obligatorio.")
            .NotNull().WithMessage("El ID de la especialidad no puede ser nulo.");

        RuleFor(x => x.CostoConsultaBase)
            .GreaterThan(0).WithMessage("El costo de la consulta base debe ser mayor que cero.")
            .LessThanOrEqualTo(9999.99m).WithMessage("El costo de la consulta base no puede exceder los 9999.99.");
    }

}
