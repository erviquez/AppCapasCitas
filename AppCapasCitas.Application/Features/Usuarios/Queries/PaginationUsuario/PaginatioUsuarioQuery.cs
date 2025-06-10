
using AppCapasCitas.Application.Specifications;
using AppCapasCitas.Application.Specifications.Usuarios;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.PaginationUsuario;

public class PaginationUsuarioQuery:UsuarioSpecificationParams, IRequest<ResponsePagination<IReadOnlyList<UsuarioResponse>>>
{

}
