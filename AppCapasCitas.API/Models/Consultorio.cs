using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

public partial class Consultorio:EntidadBaseAuditoria
{
    [Required]
    [StringLength(100)]
    public string? Nombre { get; set; }

    [Required]
    public string? Ubicacion { get; set; } // Ej: "Piso 3, Ala Norte"

    [Phone]
    public string? Telefono { get; set; }

    [StringLength(20)]
    public string? NumeroConsultorio { get; set; }

    public string? Equipamiento { get; set; } // Equipos especiales disponibles

    // Relaciones
    [Required]
    public int HospitalId { get; set; }
    public virtual Hospital? Hospital { get; set; }

    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
}
