using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Hospital: EntidadBaseAuditoria
{
    [Required]
    [StringLength(200)]
    public string? Nombre { get; set; }


    [Phone]
    public string? TelefonoPrincipal { get; set; }

    [EmailAddress]
    public string? EmailContacto { get; set; }

    public string? SitioWeb { get; set; }
    [Required]
    public string? Direccion { get; set; }
    
    [StringLength(50)]
    public string? CodigoPostal { get; set; }
    
    public string? Municipio { get; set; }

    public string? Pais { get; set; }
    [Url]
    public string Url { get; set; } = string.Empty;


    // Horarios de atención del hospital
    public string? HorarioAtencion { get; set; }
    
    // Servicios especiales del hospital
    public string? ServiciosEspeciales { get; set; }

    // Relaciones
    public virtual ICollection<Consultorio> Consultorios { get; set; } = new HashSet<Consultorio>();
    // public virtual ICollection<Medico> Medicos { get; set; } = new HashSet<Medico>();

    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new HashSet<MedicoEspecialidadHospital>();
}