using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.EditMedico;

public class EditMedicoCommand: IRequest<Response<bool>>
{
    public Guid MedicoId { get; set; } 
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }
}

