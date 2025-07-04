using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ConvertUsuario;

public class ConvertUsuarioCommand : IRequest<Response<bool>>
{
    public Guid UsuarioId { get; set; }
    public Guid RoleId { get; set; }
    public string? UsuarioAccion { get; set; }

}
