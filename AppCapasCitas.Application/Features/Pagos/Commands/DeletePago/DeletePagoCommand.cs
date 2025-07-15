using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.DeletePago;

public class DeletePagoCommand : IRequest<Response<bool>>
{
    public Guid PagoId { get; set; }

    public DeletePagoCommand(Guid pagoId)
    {
        PagoId = pagoId;
    }
}