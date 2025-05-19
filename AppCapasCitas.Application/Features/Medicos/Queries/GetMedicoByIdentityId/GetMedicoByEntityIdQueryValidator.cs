using FluentValidation;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByIdentityId;

public class GetMedicoByEntityIdQueryValidator : AbstractValidator<GetMedicoByIdentityIdQuery>
{
    public GetMedicoByEntityIdQueryValidator()
    {
        RuleFor(x => x.IdentityId)
            .NotEmpty().WithMessage("El Id de la entidad no puede estar vacío")
            .NotEqual(Guid.Empty).WithMessage("El Id de la entidad no puede ser un GUID vacío");
    } 

}