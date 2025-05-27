using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByName;

public class GetMedicoByNameQueryValidator : AbstractValidator<GetMedicoByNameQuery>
{
    public GetMedicoByNameQueryValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre no puede estar vacio")
            .NotNull().WithMessage("El nombre no puede ser nulo");
        //     .MinimumLength(1).WithMessage("el nombre es demasiado corto")
        //     .MaximumLength(255).WithMessage("el nombre es demasiado largo");
        // RuleFor(x => x.Apellido)
        //     .Empty().WithMessage("El apellido no puede estar vacio")
        //     .NotNull().WithMessage("El apellido no puede ser nulo")
        //     .MinimumLength(1).WithMessage("El apellido es demasiado corto")
        //     .MaximumLength(1000).WithMessage("El apellido es demasiado largo");
    }
    

}
