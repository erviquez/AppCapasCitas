using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;

public class CreateMedicoCommand : IRequest<Response<Guid>>
{
    public Guid UsuarioIdentityId { get; set; } 
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }
}


