using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.CreateEspecialidad;

public class CreateEspecialidadCommand:IRequest<Response<Guid>>
{
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? CostoConsultaBase { get; set; }
    public bool Activo { get; set; }
    public Guid? UsuarioCreacionId { get; set; }
}
