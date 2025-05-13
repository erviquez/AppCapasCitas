using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class Pago
{
    public int Id { get; set; }

    public decimal Monto { get; set; }

    public DateTime FechaPago { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Comprobante { get; set; }

    public string? Notas { get; set; }



    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public int? CitaId { get; set; }
    public virtual Cita? Cita { get; set; }

    public int PacienteId { get; set; }
    public virtual Paciente Paciente { get; set; } = null!;
}
