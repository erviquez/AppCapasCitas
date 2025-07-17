
namespace AppCapasCitas.DTO.Response.Usuario;

public class UsuarioResponse
{
    //usuario
    public Guid UsuarioId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Genero{ get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public int CodigoPais { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    

    public string Estado { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string? CreadoPor { get; set; }
    public string? ModificadoPor { get; set; }

 

}
