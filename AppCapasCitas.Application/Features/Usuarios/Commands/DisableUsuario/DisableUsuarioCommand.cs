using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.DisableUsuario;

public class DisableUsuarioCommand : IRequest<Response<bool>>
{
    public Guid IdentityId { get; set; }
    public bool Active { get; set; }
}
