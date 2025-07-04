using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;

public class ActionLogUsuarioCommand:IRequest<Response<bool>>
{
    public Guid UsuarioId { get; set; }
    public string? TipoAccion { get; set; }
    public string? UsuarioCreacion { get; set; }
    public string? Descripcion { get; set; }
}
