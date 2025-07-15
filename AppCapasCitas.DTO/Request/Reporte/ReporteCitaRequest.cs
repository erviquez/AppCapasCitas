using System;

namespace AppCapasCitas.DTO.Request.Reporte;

public class ReporteCitaRequest:BasePaginaConfiguracion
{
    public Guid? PacienteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadId { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
    public string? Estado { get; set; }
    public bool IncluirCostos { get; set; } = true;
    public string FormatoSalida { get; set; } = "PDF";
}
