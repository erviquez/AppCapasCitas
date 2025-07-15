using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.CreateEspecialidad;

public class CreateEspecialidadCommandValidator : AbstractValidator<CreateEspecialidadCommand>
{
    public CreateEspecialidadCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la especialidad es requerido.")
            .MaximumLength(100).WithMessage("El nombre de la especialidad no puede exceder los 100 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción de la especialidad no puede exceder los 500 caracteres.");

        RuleFor(x => x.CostoConsultaBase)
            .GreaterThan(0).WithMessage("El costo de la consulta base debe ser mayor que cero.")
            .LessThanOrEqualTo(9999.99m).WithMessage("El costo de la consulta base no puede exceder los 9999.99.");

    }

}
