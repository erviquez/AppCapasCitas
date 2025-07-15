using System;

namespace AppCapasCitas.DTO.Request.Especialidad;

public class EspecialidadUpdateRequest
{
    public Guid EspecialidadId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? CostoConsultaBase { get; set; }
    public bool Activo { get; set; }
    public Guid?usuarioCreacionId { get; set; }
}
