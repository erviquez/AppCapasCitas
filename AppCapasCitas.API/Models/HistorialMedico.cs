using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class HistorialMedico
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Diagnostico { get; set; } = null!;

    public string? Tratamiento { get; set; }

    public string? Notas { get; set; }

    public string? PresionArterial { get; set; }

    public decimal Temperatura { get; set; }

    public decimal Peso { get; set; }

    public decimal Altura { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? CitaId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual Cita? Cita { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
