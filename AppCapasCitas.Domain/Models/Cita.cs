
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Cita: EntidadBaseAuditoria
{
    [Required]
    public DateTime FechaHora { get; set; }

    [Required]
    [StringLength(200)]
    public string? Motivo { get; set; }

    [Required]
    [StringLength(20)]
    public string? Estado { get; set; } = "Programada"; // Programada, Confirmada, EnProgreso, Cancelada, Completada
    public string? Notas { get; set; }
    public string? Diagnostico { get; set; }
    public string? Tratamiento { get; set; }

    // Relaciones
    [Required]
    public int PacienteId { get; set; }
    public virtual Paciente? Paciente { get; set; }

    [Required]
    public int MedicoId { get; set; }
    public virtual Medico? Medico { get; set; }

    public int? ConsultorioId { get; set; }
    public virtual Consultorio? Consultorio { get; set; }

    public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
    public virtual Pago? Pago { get; set; }
}
