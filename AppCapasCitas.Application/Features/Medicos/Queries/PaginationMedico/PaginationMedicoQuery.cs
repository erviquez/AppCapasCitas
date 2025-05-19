using System;
using AppCapasCitas.Application.Features.Medicos.Vms.Response;
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.Application.Specifications;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;

public class PaginationMedicoQuery:SpecificationParams,IRequest<ResponsePagination<IReadOnlyList<MedicoResponse>>>
{

}
