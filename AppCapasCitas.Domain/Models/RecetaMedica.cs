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
        
        public Guid MedicoId { get; set; }
        public virtual Medico? MedicoNavigation { get; set; }

        [Required]
        public Guid PacienteId { get; set; }
        public virtual Paciente? PacienteNavigation { get; set; }

        public Guid CitaId { get; set; }
        public virtual Cita? CitaNavigation { get; set; }

        public virtual ICollection<MedicamentoRecetado> Medicamentos { get; set; } = new HashSet<MedicamentoRecetado>();
    }