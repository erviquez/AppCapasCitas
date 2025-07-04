using System;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.GetPacienteById;

public class GetPacienteByIdQuery : IRequest<Response<PacienteResponse>>
{
    public Guid PacienteId { get; set; }

    public GetPacienteByIdQuery(Guid pacienteId)
    {
        PacienteId = pacienteId;
    }

}
