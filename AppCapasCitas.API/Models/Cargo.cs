using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Cargo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int NivelJerarquico { get; set; }

    public bool EsJefatura { get; set; }

    public int? EspecialidadId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    // public virtual Especialidad? Especialidad { get; set; }

    public virtual ICollection<MedicoEspecialidadHospital> MedicoEspecialidadHospitals { get; set; } = new List<MedicoEspecialidadHospital>();
}
