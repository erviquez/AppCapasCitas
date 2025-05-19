using System;
using AppCapasCitas.Application.Features.Medicos.Vms.Response;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByEntityId.GetMedicoById;

public class GetMedicoByIdQuery:IRequest<Response<MedicoResponse>>
{
    public GetMedicoByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}
