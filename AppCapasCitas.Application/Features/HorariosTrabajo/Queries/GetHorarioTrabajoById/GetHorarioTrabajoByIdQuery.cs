using System;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.HorariosTrabajo.Queries.GetHorarioTrabajoById;

public class GetHorarioTrabajoByIdQuery : IRequest<Response<HorarioTrabajoResponse>>
{
    public Guid HorarioTrabajoId { get; set; }
    public GetHorarioTrabajoByIdQuery(Guid horarioTrabajoId)
    {
        HorarioTrabajoId = horarioTrabajoId;
    }
}

