using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Medicos.Commands.EditMedico;

public class EditMedicoCommandValidator: AbstractValidator<EditMedicoCommand>
    {
        public EditMedicoCommandValidator()
        {
            RuleFor(p => p.MedicoId)
                .NotEmpty().WithMessage("El ID de usuario es requerido")
                .NotEqual(Guid.Empty).WithMessage("El ID de usuario no puede ser un GUID vacío");

            // RuleFor(p => p.CedulaProfesional)
            //     .NotEmpty().WithMessage("La cédula profesional es requerida")
            //     .Matches(@"^[0-9]{4,20}$").WithMessage("La cédula profesional debe contener solo números y tener entre 4 y 20 dígitos");

            // RuleFor(p => p.Biografia)
            //     .MaximumLength(500).WithMessage("La biografía no puede exceder los 500 caracteres");
        }
    }