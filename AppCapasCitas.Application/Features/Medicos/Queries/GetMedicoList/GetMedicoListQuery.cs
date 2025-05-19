using System;
using AppCapasCitas.Application.Features.Medicos.Vms.Response;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoList;

public class GetMedicoListQuery:IRequest<Response<IReadOnlyList<MedicoResponse>>>
{

}
