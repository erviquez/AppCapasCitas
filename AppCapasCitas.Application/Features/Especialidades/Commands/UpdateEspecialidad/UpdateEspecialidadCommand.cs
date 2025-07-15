using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidad;

public class UpdateEspecialidadCommand : IRequest<Response<bool>>
{
    public Guid EspecialidadId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? CostoConsultaBase { get; set; }
    public bool Activo { get; set; }
    public Guid? UsuarioModificacionId { get; set; }
    

}
