using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByEntityId.GetMedicoById;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoById;

public class GetMedicoByIdQueryValidator:AbstractValidator<GetMedicoByIdQuery>
{    
    public GetMedicoByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id no puede estar vacío")
            .GreaterThan(0).WithMessage("El Id debe ser mayor que cero");
    }
}

