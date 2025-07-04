using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Pago: EntidadBaseAuditoria
    {
        [Required]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Required]
        [StringLength(20)]
        public string? MetodoPago { get; set; } // Efectivo, Tarjeta, Transferencia

        [Required]
        [StringLength(20)]
        public string? Estado { get; set; } // Pendiente, Completado, Reembolsado, Fallido

        public string? Comprobante { get; set; }
        public string? Notas { get; set; }

    // Relaciones
    [Required]
        public Guid PacienteId { get; set; }  
        public virtual Paciente? PacienteNavigation { get; set; }

        public Guid? CitaId { get; set; }
        public virtual Cita? CitaNavigation { get; set; }
    }