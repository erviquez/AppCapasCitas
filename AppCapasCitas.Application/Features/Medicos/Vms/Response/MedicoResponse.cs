

namespace AppCapasCitas.Application.Features.Medicos.Vms.Response;

public class MedicoResponse
{
    public int MedicoId { get; set; }
    public Guid? IdentityId { get; set; }
     public string? Email  { get; set; }
     public string? Nombre  { get; set; }

    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }
    public int EspecialidadId { get; set; }
    public string? NombreEspecialidad { get; set; }
    public bool Activo { get; set; }
}
