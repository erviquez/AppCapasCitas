using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Especialidad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    // public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();

    // public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();

    // Relación muchos-a-muchos con Médico y Hospital
    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();

    public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
}
