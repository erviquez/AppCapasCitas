using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidad;

public class UpdateEspecialidadCommandValidator : AbstractValidator<UpdateEspecialidadCommand>
{
    public UpdateEspecialidadCommandValidator()
    {
        RuleFor(x => x.EspecialidadId)
            .NotEmpty().WithMessage("El ID de la especialidad es obligatorio.")
            .NotNull().WithMessage("El ID de la especialidad no puede ser nulo.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la especialidad es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre de la especialidad no puede exceder los 100 caracteres.")
            .MinimumLength(3).WithMessage("El nombre de la especialidad debe tener al menos 3 caracteres.")
            ;

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción de la especialidad no puede exceder los 500 caracteres.")
            .MinimumLength(10).WithMessage("La descripción de la especialidad debe tener al menos 10 caracteres.");

        RuleFor(x => x.CostoConsultaBase)
            .GreaterThan(0).WithMessage("El costo de la consulta base debe ser mayor que cero.")
            .LessThanOrEqualTo(9999.99m).WithMessage("El costo de la consulta base no puede exceder los 9999.99.");
    }

}
