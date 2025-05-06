using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class HorarioTrabajo
{
    public int Id { get; set; }

    public int DiaSemana { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int MedicoId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual Medico Medico { get; set; } = null!;
}
