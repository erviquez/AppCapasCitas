using System;
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.DTO.Response.Hospital;

namespace AppCapasCitas.DTO.Response.Medico;

public class MedicoEspecialidadHospitalResponse
{
    public Guid MedicoEspecialidadHospitalId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    //public decimal? CostoConsultaEspecifico { get; set; }
    public string? HorarioAtencion { get; set; }
    
    // Número de contrato o identificación institucional
    public string? NumeroContrato { get; set; }
    
    // Tipo de contratación (Tiempo completo, medio tiempo, etc.)
    public string? TipoContratacion { get; set; }
 
    
    public MedicoResponse? MedicoResponse { get; set; }
    public EspecialidadResponse? EspecialidadResponse { get; set; }    
    public  HospitalResponse? HospitalResponse { get; set; }
    public CargoResponse? CargoResponse { get; set; }
}
