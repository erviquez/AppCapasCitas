using System;

namespace AppCapasCitas.DTO.Response.Cita;

public class TipoConsultaResponse
{
    public Guid TipoConsultaId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal MultiplicadorCosto { get; set; }
    public int DuracionMinutos { get; set; }
}
