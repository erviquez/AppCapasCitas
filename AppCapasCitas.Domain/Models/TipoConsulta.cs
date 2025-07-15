using System;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class TipoConsulta : EntidadBaseAuditoria
{
    [Required]
    [StringLength(50)]
    public string? Nombre { get; set; } // Primera vez, Seguimiento, Urgencia, Virtual, Domicilio

    [StringLength(200)]
    public string? Descripcion { get; set; }

    // Multiplicador de costo (1.0 = normal, 1.5 = 50% más caro, etc.)
    [Range(0.1, 5.0)]
    public decimal MultiplicadorCosto { get; set; } = 1.0M;

    // Duración típica en minutos
    public int DuracionMinutos { get; set; } = 30;

    // Propiedades de navegación
    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
}