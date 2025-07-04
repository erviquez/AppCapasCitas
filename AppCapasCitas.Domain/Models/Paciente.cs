
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Paciente : EntidadBaseAuditoria
{
    public DateTime? FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public string? Alergias { get; set; }
    public string? EnfermedadesCronicas { get; set; }
    public string? MedicamentosActuales { get; set; }
    public string? AntecedentesFamiliares { get; set; }
    public string? AntecedentesPersonales { get; set; }
    public string? Observaciones { get; set; }
    public string? Sexo { get; set; } // Nuevo campo para el sexo del paciente
    public string? TipoSangre { get; set; }
    public string? NumeroIdentificacion { get; set; }
    public string? Ciudad { get; set; }
    public string? CodigoPostal { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Ocupacion { get; set; }
    public string? Nacionalidad { get; set; }
    public string? Idiomas { get; set; }

    // Relaciones
    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
    public virtual ICollection<HistorialMedico> HistorialMedico { get; set; } = new HashSet<HistorialMedico>();
    public virtual ICollection<Pago> Pagos { get; set; } = new HashSet<Pago>();
    public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
    public virtual ICollection<Aseguradora> Aseguradoras { get; set; } = new HashSet<Aseguradora>();
    public virtual ICollection<Contacto> Contactos { get; set; } = new HashSet<Contacto>();

    public virtual Usuario? UsuarioNavigation { get; set; }
    //relacion tipo de sangre
    public Guid? TipoSangreId { get; set; }
    public virtual TipoSangre? TipoSangreNavigation { get; set; }
   
        
}