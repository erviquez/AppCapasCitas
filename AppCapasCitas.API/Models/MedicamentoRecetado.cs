using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.API.Models.Common;

namespace AppCapasCitas.API.Models;

public partial class MedicamentoRecetado: EntidadBaseAuditoria
    {
        [Required]
        [StringLength(100)]
        public string? NombreMedicamento { get; set; }

        [Required]
        public string? Dosis { get; set; }

        [Required]
        public string? Frecuencia { get; set; }

        public string? InstruccionesEspeciales { get; set; }

        // Relaciones
        [Required]
        public int RecetaMedicaId { get; set; }
        public virtual RecetaMedica? RecetaMedica { get; set; }
    }