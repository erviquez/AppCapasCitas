using System;
using FluentValidation;

namespace AppCapasCitas.Application.Features.Pagos.Queries.GetPagoById;

public class GetPagoByIdQueryValidator:AbstractValidator<GetPagoByIdQuery>
{
    public GetPagoByIdQueryValidator()
    {
        RuleFor(x => x.PagoId)
            .NotEmpty().WithMessage("El Id del pago es requerido.");
    }   
}
