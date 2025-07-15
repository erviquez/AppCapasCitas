namespace AppCapasCitas.DTO.Request.Usuario;

public class UsuarioRequest
{
    public Guid UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Genero{ get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int CodigoPais { get; set; }
    public bool Activo { get; set; }
    public string? UsuarioAccion { get; set; }


}
