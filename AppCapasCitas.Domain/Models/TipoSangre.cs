using System;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public class TipoSangre : EntidadBase
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Antigenos { get; set; }
    public string? Anticuerpos { get; set; }
    public string? RecibirDe { get; set; }
    public string? DonarA { get; set; }
    
    // Relación con Paciente
    public virtual ICollection<Paciente> Pacientes { get; set; } = new HashSet<Paciente>();

}
