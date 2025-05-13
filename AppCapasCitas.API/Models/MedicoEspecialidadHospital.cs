using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

public partial class MedicoEspecialidadHospital: EntidadBaseAuditoria
{
    

    // Propiedades específicas de la relación
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public decimal? CostoConsultaEspecifico { get; set; }

    // Propiedades adicionales
    public string? Consultorio { get; set; }
    public string? HorarioAtencion { get; set; }
    
    // Número de contrato o identificación institucional
    public string? NumeroContrato { get; set; }
    
    // Tipo de contratación (Tiempo completo, medio tiempo, etc.)
    public string? TipoContratacion { get; set; }

    //relaciones
    [Required]
    public int MedicoId { get; set; }
    public virtual Medico? Medico { get; set; }

    [Required]
    public int EspecialidadId { get; set; }
    public virtual Especialidad? Especialidad { get; set; }

    [Required]
    public int HospitalId { get; set; }
    public virtual Hospital? Hospital { get; set; }

    // Relación con Cargo (reemplaza el string Cargo)
    [Required]
    public int CargoId { get; set; }
    public virtual Cargo? Cargo { get; set; }
}