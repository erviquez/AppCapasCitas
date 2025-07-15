
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Cita : EntidadBaseAuditoria
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


    [Range(0.01, 9999.99)]
    public decimal? CostoConsulta { get; set; } // Costo final calculado


    // Relaciones
    [Required]

    public Guid PacienteId { get; set; }

    public virtual Paciente? Paciente { get; set; }

    [Required]
    public Guid MedicoId { get; set; }
    public virtual Medico? MedicoNavigation { get; set; }
    public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
    public Guid? ConsultorioId { get; set; }
    public virtual Consultorio? ConsultorioNavigation { get; set; }
    public Guid? PagoId { get; set; }
    public virtual Pago? PagoNavigation { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public virtual TipoConsulta? TipoConsultaNavigation { get; set; }
}
