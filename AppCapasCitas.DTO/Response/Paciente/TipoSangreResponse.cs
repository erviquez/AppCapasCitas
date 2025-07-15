using System;

namespace AppCapasCitas.DTO.Response.Paciente;

public class TipoSangreResponse
{
    public Guid TipoSangreId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Antigenos { get; set; }
    public string? Anticuerpos { get; set; }
    public string? RecibirDe { get; set; }
    public string? DonarA { get; set; }
}
