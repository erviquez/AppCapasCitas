using System;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioByIdentityId;

public class GetUsuarioByIdentityIdQuery : IRequest<Response<UsuarioResponse>>
{
    public Guid UsuarioId { get; set; }

}
