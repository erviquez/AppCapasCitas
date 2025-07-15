using System;
using AppCapasCitas.DTO.Response.Medico;

namespace AppCapasCitas.DTO.Response.Hospital;

public class HospitalResponse
{
    public Guid HospitalId { get; set; }
    public string? Nombre { get; set; }
    public string? TelefonoPrincipal { get; set; }
    public string? EmailContacto { get; set; }
    public string? SitioWeb { get; set; }
    public string? Direccion { get; set; }
    public string? CodigoPostal { get; set; }
    
    public string? Ciudad { get; set; }
    public string? Estado { get; set; }
    public string? CodigoPais { get; set; }


    public string? Pais { get; set; }
    public string? Url { get; set; } 
    public string? HorarioAtencion { get; set; }    
    public string? ServiciosEspeciales { get; set; }

    // Relaciones
    public  List<ConsultorioResponse>? ListConsultorioResponse { get; set; } 
    public  List<MedicoEspecialidadHospitalResponse>? ListMedicoEspecialidadHospitalResponse { get; set; } 
}
