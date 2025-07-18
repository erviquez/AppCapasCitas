﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

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
        public Guid RecetaMedicaId { get; set; }
        public virtual RecetaMedica? RecetaMedicaNavigation { get; set; }
    }