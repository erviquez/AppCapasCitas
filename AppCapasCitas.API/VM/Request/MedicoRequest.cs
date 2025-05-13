using System;

namespace AppCapasCitas.API.VM.Request;

public class MedicoRequest
{
    public string CedulaProfesional { get; set; } = null!;
    public string Biografia { get; set; } = null!;
    public int? EspecialidadId { get; set; }
    public int? HospitalId { get; set; }


}
