

using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.DTO.Response.Usuario;

namespace AppCapasCitas.DTO.Response.Medico;

public class MedicoResponse
{

    //medico
    public Guid MedicoId { get; set; }
    public string CedulaProfesional { get; set; } = null!;
    public string Biografia { get; set; } = null!;
    public string? Universidad { get; set; } 
    public UsuarioResponse? UsuarioResponse { get; set; }    
    public List<HorarioTrabajoResponse>? ListHorarioTrabajoResponse { get; set; } 
    public List<EspecialidadResponse>? ListEspecialidadResponse { get; set; } 
    
    

}
