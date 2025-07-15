
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadList;

public class GetEspecialidadListQuery:IRequest<Response<List<EspecialidadResponse>>>
{

}
