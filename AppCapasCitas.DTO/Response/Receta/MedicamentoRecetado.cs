using System;

namespace AppCapasCitas.DTO.Response.Receta;

public class MedicamentoRecetado
{
    public Guid MedicamentoRecetadoId { get; set; }
    public string? NombreMedicamento { get; set; } // Nombre del medicamento
    public string? Dosis { get; set; } // Dosis recomendada
    public string? Frecuencia { get; set; } // Frecuencia de administración
    public string? InstruccionesEspeciales { get; set; } // Instrucciones adicionales
    public Guid RecetaMedicaId { get; set; } // Relación con RecetaMedica
}
