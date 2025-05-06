using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Cita
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public string Motivo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Notas { get; set; }

    public string? Diagnostico { get; set; }

    public string? Tratamiento { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? ConsultorioId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual Consultorio? Consultorio { get; set; }

    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();

    public virtual Medico Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Pago? Pago { get; set; }

    public virtual ICollection<RecetaMedica> RecetaMedicas { get; set; } = new List<RecetaMedica>();
}
