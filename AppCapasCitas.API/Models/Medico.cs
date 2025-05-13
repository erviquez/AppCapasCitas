using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

public partial class Medico: EntidadBaseAuditoria
{

    [Required]
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }

    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();
    public virtual ICollection<HorarioTrabajo> HorariosTrabajo { get; set; } = new HashSet<HorarioTrabajo>();
    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();
    public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
    public virtual Usuario? Usuario { get; set; }
}
