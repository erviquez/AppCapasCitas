namespace AppCapasCitas.DTO.Request.Medico;

public class MedicoRequest
{
    public Guid MedicoId { get; set; }
    public string CedulaProfesional { get; set; } = null!;
    public string Biografia { get; set; } = null!;



}
