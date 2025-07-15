using System;
using AppCapasCitas.DTO.Response.Cita;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Queries.GetPagoById;

public class GetPagoByIdQuery : IRequest<Response<PagoResponse>>
{
    public Guid PagoId { get; set; }

    public GetPagoByIdQuery(Guid pagoId)
    {
        PagoId = pagoId;
    }
}