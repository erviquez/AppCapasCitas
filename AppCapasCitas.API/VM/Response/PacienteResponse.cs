
namespace AppCapasCitas.API.VM.Response;

public class PacienteResponse
{
    //usuario
    public int Id { get; set; }
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
    public DateTime? UltimoLogin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string? CreadoPor { get; set; }
    public string? ModificadoPor { get; set; }
    public string Email { get; set; } = null!;

    //paciente
    public int PacienteId { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Genero { get; set; } = string.Empty;
    public string? Alergias { get; set; }
    public string? EnfermedadesCronicas { get; set; }
    public string? MedicamentosActuales { get; set; }
}
