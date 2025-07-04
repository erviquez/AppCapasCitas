
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Medico: EntidadBaseAuditoria
{

    
    public string? CedulaProfesional { get; set; }
    public string? Biografia { get; set; }

    public virtual ICollection<Cita> Citas { get; set; } = new HashSet<Cita>();
    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();
    public virtual ICollection<HorarioTrabajo> HorariosTrabajo { get; set; } = new HashSet<HorarioTrabajo>();
    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; } = new List<MedicoEspecialidadHospital>();
    public virtual ICollection<RecetaMedica> RecetasMedicas { get; set; } = new HashSet<RecetaMedica>();
    //public Guid? UsuarioId { get; set; }  
    public virtual Usuario? UsuarioNavigation { get; set; }
}
