using System;
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadById;

public class GetEspecialidadByIdQuery : IRequest<Response<EspecialidadResponse>>
{
    public Guid EspecialidadId { get; set; }

    public GetEspecialidadByIdQuery(Guid especialidadId)
    {
        EspecialidadId = especialidadId;
    }

}
