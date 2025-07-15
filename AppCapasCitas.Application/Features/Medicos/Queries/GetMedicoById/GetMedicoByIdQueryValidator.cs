using FluentValidation;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoById;

public class GetMedicoByIdQueryValidator : AbstractValidator<GetMedicoByIdQuery>
{
    public GetMedicoByIdQueryValidator()
    {
        RuleFor(x => x.IdentityId)
            .NotEmpty().WithMessage("El Id de la entidad no puede estar vacío")
            .NotEqual(Guid.Empty).WithMessage("El Id de la entidad no puede ser un GUID vacío");
    } 

}