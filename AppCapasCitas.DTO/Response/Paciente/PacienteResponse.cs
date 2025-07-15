using AppCapasCitas.DTO.Response.Usuario;

namespace AppCapasCitas.DTO.Response.Paciente;

public class PacienteResponse
{
    //paciente
    public DateTime? FechaNacimiento { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Ocupacion { get; set; }
    public string? Genero { get; set; }    public string? Alergias { get; set; }
    public string? EnfermedadesCronicas { get; set; }
    public string? MedicamentosActuales { get; set; }
    public string? AntecedentesFamiliares { get; set; }
    public string? AntecedentesPersonales { get; set; }
    public string? Observaciones { get; set; }

    public UsuarioResponse? UsuarioResponse { get; set; }
    public TipoSangreResponse? TipoSangreResponse { get; set; }
    public List<ContactoResponse>? ListContactoResponse { get; set; } 
    public List<AseguradoraResponse>? ListAseguradoraResponse { get; set; }

}
