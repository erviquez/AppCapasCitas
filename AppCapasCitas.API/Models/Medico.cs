using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Medico
{
    public int Id { get; set; }

    public string CedulaProfesional { get; set; } = null!;

    public string? Biografia { get; set; }

    // public int? EspecialidadId { get; set; }

    // public int? HospitalId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    
    
    
    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();

    public virtual ICollection<HorarioTrabajo> HorarioTrabajos { get; set; } = new List<HorarioTrabajo>();

    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();

    public virtual ICollection<RecetaMedica> RecetaMedicas { get; set; } = new List<RecetaMedica>();

    public virtual Usuario? Usuario { get; set; }
}
