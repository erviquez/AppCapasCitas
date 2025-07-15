
using AppCapasCitas.Application.Specifications.Pacientes;
using AppCapasCitas.DTO.Response.Paciente;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.PaginationPaciente;

public class PaginationPacienteQuery:PacienteSpecificationParams,IRequest<ResponsePagination<IReadOnlyList<PacienteResponse>>>
{

}
