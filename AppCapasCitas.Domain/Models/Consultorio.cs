﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

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
    public Guid HospitalId { get; set; }
    public virtual Hospital? HospitalNavigation { get; set; }

    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
}
