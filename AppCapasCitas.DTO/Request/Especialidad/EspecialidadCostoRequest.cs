using System;

namespace AppCapasCitas.DTO.Request.Especialidad;

public class EspecialidadCostoRequest
{
    public Guid EspecialidadId { get; set; }
    public decimal CostoConsultaBase { get; set; }
    public Guid? UsuarioModificacionId { get; set; }

}
