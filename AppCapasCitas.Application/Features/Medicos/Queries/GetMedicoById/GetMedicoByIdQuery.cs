
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoById;

public class GetMedicoByIdQuery:IRequest<Response<MedicoResponse>>
{
    public GetMedicoByIdQuery(Guid identityId)
    {
        IdentityId = identityId;
    }
    public Guid IdentityId { get; set; }
}
