using System;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Identity;
using MediatR;

namespace AppCapasCitas.Application.Features.Identity.Queries.GetRoles;

public class GetRolesQuery:IRequest<Response<List<Role>>>
{

}
