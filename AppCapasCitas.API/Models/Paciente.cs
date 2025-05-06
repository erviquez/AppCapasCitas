using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Paciente
{
    public int Id { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Genero { get; set; } = null!;

    public string? Alergias { get; set; }

    public string? EnfermedadesCronicas { get; set; }

    public string? MedicamentosActuales { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<RecetaMedica> RecetaMedicas { get; set; } = new List<RecetaMedica>();

    public virtual Usuario? Usuario { get; set; }
}
