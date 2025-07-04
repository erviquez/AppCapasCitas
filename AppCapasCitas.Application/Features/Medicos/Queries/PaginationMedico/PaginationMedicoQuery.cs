
using AppCapasCitas.Application.Specifications.Medicos;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;

public class PaginationMedicoQuery:MedicoSpecificationParams,IRequest<ResponsePagination<IReadOnlyList<MedicoResponse>>>
{

}
