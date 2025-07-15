using System;
using AppCapasCitas.DTO.Response.Cita;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Queries.GetPagosByPaciente;

public class GetPagosByPacienteQuery : IRequest<ResponseGeneric<IEnumerable<PagoResponse>>>
{
    public Guid PacienteId { get; set; }

    public GetPagosByPacienteQuery(Guid pacienteId)
    {
        PacienteId = pacienteId;
    }
}