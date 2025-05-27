

using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;

public class GetUsuarioListQuery:IRequest<Response<IReadOnlyList<UsuarioResponse>>>

{

}
