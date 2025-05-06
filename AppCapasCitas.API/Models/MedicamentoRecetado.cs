using System;
using System.Collections.Generic;

namespace AppCapasCitas.API.Models;

public partial class MedicamentoRecetado
{
    public int Id { get; set; }

    public string NombreMedicamento { get; set; } = null!;

    public string Dosis { get; set; } = null!;

    public string Frecuencia { get; set; } = null!;

    public string? InstruccionesEspeciales { get; set; }

    public int RecetaMedicaId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? CreadoPor { get; set; }

    public string? ModificadoPor { get; set; }

    public bool Activo { get; set; }

    public virtual RecetaMedica RecetaMedica { get; set; } = null!;
}
