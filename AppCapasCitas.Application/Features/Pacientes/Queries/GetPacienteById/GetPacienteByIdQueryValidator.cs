using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.GetPacienteById;

public class GetPacienteByIdQueryValidator:AbstractValidator<GetPacienteByIdQuery>
{
    public GetPacienteByIdQueryValidator()
    {
        RuleFor(x => x.PacienteId)
            .NotEmpty().WithMessage("El Id del paciente es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del paciente no puede ser un GUID vacío.");
    }

}
