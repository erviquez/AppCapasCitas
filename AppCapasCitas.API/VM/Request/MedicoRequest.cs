using System;

namespace AppCapasCitas.API.VM.Request;

public class MedicoRequest
{
     public int MedicoId { get; set; }
    public string CedulaProfesional { get; set; } = null!;
    public string Biografia { get; set; } = null!;
    public int? EspecialidadId { get; set; }

    public int? HospitalId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string? CreadoPor { get; set; }
    public string? ModificadoPor { get; set; }

}
