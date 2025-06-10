
namespace AppCapasCitas.DTO.Response.Usuario;

public class UsuarioResponse
{
    //usuario
    public Guid? IdentityId { get; set; }
    public string Email { get; set; } = null!;
    
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Celular { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public int CodigoPais { get; set; }
    public string Pais { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public bool Activo { get; set; }
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string? CreadoPor { get; set; }
    public string? ModificadoPor { get; set; }
    // //UsuarioIdentity
    // public Guid? IdentityId { get; set; }
    // public bool UsuarioActivo { get; set; }
    // public DateTime? LastLogin { get; set; } = null;
    // public string Email { get; set; } = null!;


}
