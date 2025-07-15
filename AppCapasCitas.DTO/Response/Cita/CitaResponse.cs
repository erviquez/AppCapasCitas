using System;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.DTO.Response.Paciente;
using AppCapasCitas.DTO.Response.Receta;

namespace AppCapasCitas.DTO.Response.Cita;

public class CitaResponse
{
    public Guid CitaId { get; set; }
    public DateTime FechaHora { get; set; }
    public string? Motivo { get; set; }
    public string? Estado { get; set; }
    public string? Notas { get; set; }
    public string? Diagnostico { get; set; }
    public string? Tratamiento { get; set; }
    
    // ✅ Nuevos campos para costos
    public decimal? CostoConsulta { get; set; }

    public MedicoResponse? MedicoResponse { get; set; }
    public  List<RecetaMedicaResponse>? ListRecetaMedicaResponse { get; set; }
     public  ConsultorioResponse? ConsultorioResponse { get; set; }
    public  PacienteResponse? PacientePacienteResponse { get; set; }
    public  PagoResponse? PagoResponse { get; set; }
}
