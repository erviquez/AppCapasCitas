using System.ComponentModel.DataAnnotations;

namespace AppCapasCitas.Domain.Models.Common;

public abstract class EntidadBaseAuditoria
{
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime? FechaActualizacion { get; set; }

        public string? CreadoPor { get; set; } = "system";

        public string? ModificadoPor { get; set; }
        public bool Activo { get; set; } = true;
}
