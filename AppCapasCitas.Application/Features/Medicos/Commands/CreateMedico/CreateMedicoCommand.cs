using System;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;

public class CreateMedicoCommand : IRequest<int>
{
    public Guid UsuarioId { get; set; } 
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }
}


