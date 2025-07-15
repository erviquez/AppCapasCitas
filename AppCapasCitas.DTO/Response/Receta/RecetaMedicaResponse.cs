using System;

namespace AppCapasCitas.DTO.Response.Receta;

public class RecetaMedicaResponse
{
    public Guid RecetaMedicaId { get; set; }
    public string? Medicamento { get; set; } // Nombre del medicamento
    public string? Dosis { get; set; } // Dosis recomendada
    public string? Instrucciones { get; set; } // Instrucciones de uso
    public DateTime FechaEmision { get; set; } // Fecha en que se emitió la receta
    public DateTime? FechaVencimiento { get; set; } // Fecha de vencimiento de la receta, si aplica
    public List<MedicamentoRecetado>? Medicamentos { get; set; } 

}
