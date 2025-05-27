
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.Application.Specifications;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;

public class PaginationMedicoQuery:SpecificationParams,IRequest<ResponsePagination<IReadOnlyList<MedicoResponse>>>
{

}
