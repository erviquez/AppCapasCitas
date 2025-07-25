﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class MedicoEspecialidadHospital: EntidadBaseAuditoria
{
    

    // Propiedades específicas de la relación
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public decimal? CostoConsultaEspecifico { get; set; }

    //relaciones
    [Required]

    public Guid MedicoId { get; set; }  
    public virtual Medico? MedicoNavigation { get; set; }

    [Required]
    public Guid EspecialidadId { get; set; }
    public virtual Especialidad? EspecialidadNavigation { get; set; }

    [Required]
    public Guid HospitalId { get; set; }
    public virtual Hospital? HospitalNavigation { get; set; }

    // Relación con Cargo (reemplaza el string Cargo)
    [Required]
    public Guid CargoId { get; set; }
    public virtual Cargo? CargoNavigation { get; set; }
}