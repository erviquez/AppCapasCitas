using System;

namespace AppCapasCitas.DTO.Request.Reporte;

public class ReporteIdRequest:BasePaginaConfiguracion
{
    public Guid Id { get; set; }
}
