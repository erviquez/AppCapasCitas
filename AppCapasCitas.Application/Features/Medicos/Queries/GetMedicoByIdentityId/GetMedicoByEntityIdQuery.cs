using System;
using AppCapasCitas.Application.Features.Medicos.Vms.Response;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByIdentityId;

public class GetMedicoByIdentityIdQuery:IRequest<Response<MedicoResponse>>
{
    public GetMedicoByIdentityIdQuery(Guid identityId)
    {
        IdentityId = identityId;
    }
    public Guid IdentityId { get; set; }
}
