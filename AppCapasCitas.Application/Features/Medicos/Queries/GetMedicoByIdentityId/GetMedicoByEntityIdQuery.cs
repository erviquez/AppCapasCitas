
using AppCapasCitas.DTO.Response.Medico;
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
