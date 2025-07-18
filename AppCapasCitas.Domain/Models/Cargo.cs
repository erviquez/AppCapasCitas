﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Cargo  : EntidadBaseAuditoria
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    // Nivel jerárquico (opcional)
    public int NivelJerarquico { get; set; } = 1;

    // Indica si es un cargo de jefatura
    public bool EsJefatura { get; set; } = false;

    // Relación con especialidad (si el cargo es específico de una especialidad)
    public Guid? EspecialidadId { get; set; }
    public virtual Especialidad? EspecialidadNavigation { get; set; }
}
