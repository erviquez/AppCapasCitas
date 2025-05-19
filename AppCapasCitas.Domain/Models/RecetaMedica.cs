using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class RecetaMedica: EntidadBaseAuditoria
    {
        [Required]
        public DateTime FechaEmision { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required]
        public string? Instrucciones { get; set; }

        // Relaciones
        [Required]
        public int MedicoId { get; set; }
        public virtual Medico? Medico { get; set; }

        [Required]
        public int PacienteId { get; set; }
        public virtual Paciente? Paciente { get; set; }

        public int? CitaId { get; set; }
        public virtual Cita? Cita { get; set; }

        public virtual ICollection<MedicamentoRecetado> Medicamentos { get; set; } = new HashSet<MedicamentoRecetado>();
    }