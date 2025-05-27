using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoList;

public class GetMedicoListQuery:IRequest<Response<IReadOnlyList<MedicoResponse>>>
{

}
