using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class RecetaMedica : EntidadBaseAuditoria
{
    [Required]
    public DateTime FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [Required]
    public string? Instrucciones { get; set; }

    // NUEVOS CAMPOS
    [MaxLength(500)]
    public string? DiagnosticoPrincipal { get; set; }

    [MaxLength(1000)]
    public string? DiagnosticosSecundarios { get; set; } // Puede ser texto libre o serializado

    [MaxLength(1000)]
    public string? Observaciones { get; set; }

    [MaxLength(250)]
    public string? Motivo { get; set; }

    [MaxLength(50)]
    public string? Estado { get; set; } // Ej: Vigente, Vencida, Anulada

    public string? Adjuntos { get; set; } // Ruta o referencia a archivos adjuntos

    // Relaciones
    [Required]
    public Guid MedicoId { get; set; }
    public virtual Medico? MedicoNavigation { get; set; }

    [Required]
    public Guid PacienteId { get; set; }
    public virtual Paciente? PacienteNavigation { get; set; }

    public Guid CitaId { get; set; }
    public virtual Cita? CitaNavigation { get; set; }

    public virtual ICollection<MedicamentoRecetado> Medicamentos { get; set; } = new HashSet<MedicamentoRecetado>();
}