using System;
using AppCapasCitas.DTO.Response.Hospital;

namespace AppCapasCitas.DTO.Response.Cita;

public class ConsultorioResponse
{
    public Guid ConsultorioId { get; set; }
    public string? Nombre { get; set; }
    public string? Ubicacion { get; set; } // Ej: "Piso 3, Ala Norte"

    public string? Telefono { get; set; }

    public string? NumeroConsultorio { get; set; }
    public string? Equipamiento { get; set; } // Equipos especiales disponibles

    //public virtual HospitalResponse? HospitalResponse { get; set; }
    //pendiente: Relacionar con citas
    //public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
    


}
