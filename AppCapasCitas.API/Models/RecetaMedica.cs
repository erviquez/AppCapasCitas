using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class RecetaMedica
{
    public int Id { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public string Instrucciones { get; set; } = null!;


    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }



    public int? CitaId { get; set; }
    public virtual Cita? Cita { get; set; }


    public int MedicoId { get; set; }
    public virtual Medico Medico { get; set; } = null!;

    public int PacienteId { get; set; }
    public virtual Paciente Paciente { get; set; } = null!;
    public virtual ICollection<MedicamentoRecetado> MedicamentoRecetados { get; set; } = new List<MedicamentoRecetado>();
}
