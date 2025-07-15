using System;

namespace AppCapasCitas.DTO.Response.Cita;

public class PagoResponse
{
    public Guid PagoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public string? MetodoPago { get; set; }
    public string? EstadoPago { get; set; }
    public string? Comprobante { get; set; }
    public string? Notas { get; set; }
    public Guid PacienteId { get; set; }
    public string? PacienteNombre { get; set; }
    public Guid? CitaId { get; set; }
    public DateTime? CitaFechaHora { get; set; }    
    public string? CitaMotivo { get; set; }
    public string? CitaDescripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string CreadoPor { get; set; } = "Sistema"; // TODO: Obtener del contexto
    public DateTime? FechaActualizacion { get; set; }
    public string? ModificadoPor { get; set; } // TODO: Obtener del contexto
    

}
