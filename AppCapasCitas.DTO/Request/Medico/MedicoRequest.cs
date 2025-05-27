namespace AppCapasCitas.DTO.Request.Medico;

public class MedicoRequest
{
    public string CedulaProfesional { get; set; } = null!;
    public string Biografia { get; set; } = null!;
    public int? EspecialidadId { get; set; }
    public int? HospitalId { get; set; }


}
